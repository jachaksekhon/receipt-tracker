import { useState } from "react";
import type { AxiosError } from "axios";
import { Link, useNavigate } from "react-router-dom";

import { Button } from "@/components/ui/button";
import LoginStrings from "@/constants/strings/LoginStrings";
import { login } from "@/apis/auth/authService";
import { BannerError, InputError } from "@/components/ErrorUI";
import IntConstants from "@/constants/ints/IntConstants";

function getAxiosErrorMessage(err: unknown) {
  const ax = err as AxiosError<{ error?: string; message?: string }>;
  return ax?.response?.data?.error || ax?.response?.data?.message || ax?.message || "Login failed.";
}

export default function Login() {
    
    const navigate = useNavigate();
    const [loggingIn, setLoggingIn] = useState(false);
    
    const [errorMsg, setErrorMessage] = useState<string | null>("Internal server error");
    const [emailError, setEmailError] = useState<string | null>("Email not found");
    const [passwordError, setPasswordError] = useState<string | null>("Password does not match backend");


    const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form     = e.currentTarget;
        const formData = new FormData(form);

        const email    = String(formData.get("email") || "").toLowerCase().trim();
        const password = String(formData.get("password") || "");

        if (!email || !password){
            alert(LoginStrings.EnterEmailOrPassword);
            return;
        }

        try {
            const { token } = await login({ email, password });
            localStorage.setItem("JWT_TOKEN", token);
            navigate("/");
        }
        catch (err) {
            const msg = getAxiosErrorMessage(err);

            if (msg == LoginStrings.backendEmailErrorKey) { setEmailError(LoginStrings.backendEmailErrorKey) }
            else if (msg == LoginStrings.backendPasswordErrorKey) { setPasswordError(LoginStrings.backendPasswordErrorKey) }
            else { setErrorMessage(msg) }
        }
        finally {
            setLoggingIn(false)
        }
    }

    return (
        <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
            <div className="sm:mx-auto sm:w-full sm:max-w-sm">
                <img className="mx-auto h-10 w-auto" src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=600" alt="Your Company" />
                <h2 className="mt-10 text-center text-2xl/9 font-bold tracking-tight text-gray-900 pb-2">{LoginStrings.Title}</h2>

                <BannerError message={errorMsg}/>

            </div>

            <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
                <form 
                    className="space-y-6" 
                    onSubmit={handleLogin}>

                        <div>
                            <label 
                                className="block text-sm/6 font-medium text-primary font-sans">
                                    {LoginStrings.EmailLabel}
                            </label>
                            <div className="mt-2">
                                <input 
                                    type="email" 
                                    name="email" 
                                    id="email" 
                                    required 
                                    maxLength={IntConstants.maxEmailLength}
                                    className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                                    onChange={() => setEmailError(null)}
                                    />
                            </div>
                            <InputError message={emailError}/>
                        </div>

                        <div>
                            <div className="flex items-center justify-between">
                                <label 
                                    className="block text-sm/6 font-medium text-primary font-sans">
                                        {LoginStrings.PasswordLabel}
                                </label>
                                <div className="text-sm">
                                    <a href="#" className="font-semibold text-primary hover:text-[#fcda00] font-sans">{LoginStrings.ForgotPassword}</a>
                                </div>
                            </div>

                            <div className="mt-2">
                            <input 
                                name="password" 
                                id="password" 
                                type="password"
                                required
                                maxLength={IntConstants.maxPasswordLength}
                                className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6" 
                                onChange={() => setPasswordError(null)}
                                />
                            </div>
                            <InputError message={passwordError}/>
                        </div>

                        <div>
                            <Button 
                                type="submit" 
                                className="w-full"
                                disabled={loggingIn}>
                                    {loggingIn ? LoginStrings.LoggingIn : LoginStrings.LoginButton}
                            </Button>
                        </div>  
                </form>

                <p className="mt-10 text-center text-sm/6 text-primary">
                    {LoginStrings.NotAMember} 
                <a href="#" className="font-semibold text-primary hover:text-[#fcda00] hover:underline ml-1">{LoginStrings.SignUp}</a>
                </p>
            </div>
        </div>
    )

}