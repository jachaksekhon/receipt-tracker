import { Request, Response } from 'express';
import { getUsers } from '../services/userService';

export const getAllUsers = async(req: Request, res: Response) => {
    try {
        const users = await getUsers();
        res.json(users);
    } catch (error) {
        res.status(500).json({error: 'Something went wrong'})
    }
}