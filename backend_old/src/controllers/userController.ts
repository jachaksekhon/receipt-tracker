import { Request, Response } from 'express';
import UserService from '../services/userService';
import { ResponseStrings } from '../constants/ResponseStrings';
import { ResponseCodes } from '../constants/ResponseCodes';

export const getAllUsers = async(req: Request, res: Response) => {
    try {
        const users = await UserService.getUsers();
        res.json(users);
    } catch (error) {
        res.status(ResponseCodes.InternalServerError).json({ error: ResponseStrings.GettingUsersError })
    }
}