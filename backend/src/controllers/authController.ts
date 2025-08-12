import { Request, Response } from 'express';
import bcrypt from 'bcrypt';
import jwt from 'jsonwebtoken';
import dotenv from 'dotenv'

import userService from '../services/userService';

dotenv.config();

const JWT_SECRET = process.env.JWT_SECRET as string;
if (!JWT_SECRET) throw new Error("JWT_SECRET is not defined");

export const signup = async (req: Request, res: Response) => {

    try {

        let { firstname, lastname, email, password } = req.body || {};

        // Basic validation

        if (!firstname || !lastname || !email || !password) {
            return res.status(400).json({error: 'Missing required fields'});
        }

        // normalize inputs

        email     = String(email).toLowerCase().trim();
        firstname = firstname.trim();
        lastname  = lastname.trim();

        // Check if email already exists
        const existing = await userService.findUserByEmail(email);

        if (existing) 
            return res.status(400).json({ error: 'An account with that email already exists!'});

        const hashedPassword = await bcrypt.hash(password, 10);

        const user = await userService.createUser({
             
            firstname, 
            lastname,
            email, 
            password: hashedPassword
            
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
    

    try {
        let { email, password } = req.body || {};

        if (!email || !password) {
            return res.status(400).json({ error: 'Missing email or password' });
        }

        email = String(email).toLowerCase().trim();

        const user = await userService.findUserByEmail(email);
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