import { Request, Response } from 'express';
import UserService from '../services/userService';
import { ResponseStrings } from '../strings/ResponseStrings';

export const getAllUsers = async(req: Request, res: Response) => {
    try {
        const users = await UserService.getUsers();
        res.json(users);
    } catch (error) {
        res.status(500).json({ error: ResponseStrings.GettingUsersError })
    }
}