import { AxiosError } from "axios"
import apiAuth from "./apiAuth";

export const signup = async (data: { firstname: string; lastname: string; email: string; password: string}) => {
    try {
        const res = await apiAuth.post("/signup", data);
        return res.data
    }
    catch(error) {
        const err = error as AxiosError;
        throw err.response?.data
    }
};

export const login = (data: { email: string, password: string }) => 
    apiAuth.post("/login", data)