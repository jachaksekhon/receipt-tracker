import { Request, Response } from 'express';
import bcrypt from 'bcrypt';
import jwt from 'jsonwebtoken';

import userService from '../services/userService';

import { ResponseStrings } from '../constants/ResponseStrings';
import { ResponseCodes } from '../constants/ResponseCodes';


const JWT_SECRET = process.env.JWT_SECRET as string;
if (!JWT_SECRET) throw new Error(ResponseStrings.JWTNotDefined);

export const signup = async (req: Request, res: Response) => {

    try {

        let { firstname, lastname, email, password } = req.body || {};

        if (!firstname || !lastname || !email || !password) {
            return res.status(ResponseCodes.BadRequest).json({ error: ResponseStrings.MissingFields });
        }

        email     = String(email).toLowerCase().trim();
        firstname = firstname.trim();
        lastname  = lastname.trim();

        const existing = await userService.findUserByEmail(email);

        if (existing) 
            return res.status(ResponseCodes.BadRequest).json({ error: ResponseStrings.DuplicateEmail });

        const hashedPassword = await bcrypt.hash(password, 10);

        const user = await userService.createUser({
             
            firstname, 
            lastname,
            email, 
            password: hashedPassword
            
        });

        const token = jwt.sign({ userId: user.id }, JWT_SECRET, { expiresIn: ResponseStrings.KeyExpiry });

        res.status(ResponseCodes.ResourceCreated).json({
            message: ResponseStrings.UserSuccess,
            token,
            user: {
                id: user.id,
                firstname: user.firstname,
                lastname: user.lastname,
                email: user.email,
                createdAt: user.createdAt
            },
        });
        
    } catch (err) {
        console.error(ResponseStrings.SignupError, err);
        res.status(ResponseCodes.InternalServerError).json({ error: ResponseStrings.InternalError });
    }
}

export const login = async (req: Request, res: Response) => {
    
    try {

        let { email, password } = req.body || {};

        if (!email || !password) {
            return res.status(ResponseCodes.BadRequest).json({ error: ResponseStrings.MissingFields });
        }

        email = String(email).toLowerCase().trim();

        const user = await userService.findUserByEmail(email);

        if (!user) {
            return res.status(ResponseCodes.Unauthorized).json({ error: ResponseStrings.EmailNotFound });
        }

        const isValid = await bcrypt.compare(password, user.password);

        if (!isValid) {
            return res.status(ResponseCodes.Unauthorized).json({ error: ResponseStrings.WrongPass });
        }

        const token = jwt.sign({ userId: user.id }, JWT_SECRET, { expiresIn: ResponseStrings.KeyExpiry });

        res.json({ message: ResponseStrings.LoginSuccess, token});

    } catch (err){
        res.status(ResponseCodes.InternalServerError).json({ error: ResponseStrings.InternalError })
    }
}