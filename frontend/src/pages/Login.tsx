import { Button } from "@/components/ui/button";
import LoginStrings from "@/strings/LoginStrings";

export default function Login() {   

    const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget
        const formData = new FormData(form)
        const email = formData.get("email")
        const password = formData.get("password")

        // implement backend connection here via fetch/axios
    }

    return (
        <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
            <div className="sm:mx-auto sm:w-full sm:max-w-sm">
                <img className="mx-auto h-10 w-auto" src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=600" alt="Your Company" />
                <h2 className="mt-10 text-center text-2xl/9 font-bold tracking-tight text-gray-900">{LoginStrings.Title}</h2>
            </div>

            <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
                <form className="space-y-6" onSubmit={handleLogin}>
                <div>
                    <label className="block text-sm/6 font-medium text-primary font-sans">{LoginStrings.EmailLabel}</label>
                    <div className="mt-2">
                    <input type="email" name="email" id="email" required className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6" />
                    </div>
                </div>

                <div>
                    <div className="flex items-center justify-between">
                    <label className="block text-sm/6 font-medium text-primary font-sans">{LoginStrings.PasswordLabel}</label>
                    <div className="text-sm">
                        <a href="#" className="font-semibold text-primary hover:text-[#fcda00] font-sans">{LoginStrings.ForgotPassword}</a>
                    </div>
                    </div>
                    <div className="mt-2">
                    <input name="password" id="password" required className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6" />
                    </div>
                </div>

                <div>
                    <Button type="submit" className="w-full">{LoginStrings.SubmitButton}</Button>
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