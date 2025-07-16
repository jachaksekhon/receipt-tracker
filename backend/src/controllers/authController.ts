import { Request, Response } from 'express';
import bcrypt from 'bcrypt';
import jwt from 'jsonwebtoken';
import { PrismaClient } from '@prisma/client';
import { createUser, findUserByEmail } from '../services/userService';
import dotenv from 'dotenv'

const prisma = new PrismaClient();
dotenv.config();

const JWT_SECRET = process.env.JWT_SECRET as string;
if (!JWT_SECRET) throw new Error("JWT_SECRET is not defined");

export const signup = async (req: Request, res: Response) => {
    const { firstname, lastname, email, password } = req.body;

    try {

        // Check if email already exists
        const existing = await prisma.user.findUnique({where: { email }});

        if (existing) 
            return res.status(400).json({ error: 'An account with that email already exists!'});

        const hashedPassword = await bcrypt.hash(password, 10);

        const user = await createUser({
            data: { 
                firstname, 
                lastname,
                email, 
                password: hashedPassword
            },
        });

        res.status(201).json({
            message: 'User created successfully',
            user: {
                id: user.id,
                firstname: user.firstname,
                lastname: user.lastname,
                email: user.email,
                createdAt: user.createdAt
            },
        });
    } catch (err) {
        console.error('Signup error:', err);
        res.status(500).json({ error: "Internal server error" });
    }
}

export const login = async (req: Request, res: Response) => {
    const { email, password } = req.body;

    try {
        const user = await findUserByEmail(email);
        if (!user) {
            return res.status(401).json({ error: "Email not found"});
        }

        const isValid = await bcrypt.compare(password, user.password);
        if (!isValid) {
            return res.status(401).json({ error: "Incorrect password!"});
        }

        const token = jwt.sign({ userId: user.id }, JWT_SECRET, { expiresIn: "1d" });
        res.json({ message: 'Login successful', token});

    } catch (err){
        res.status(500).json({ error: "Error logging in"})
    }
}