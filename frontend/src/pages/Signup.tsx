import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom"
import SignupStrings from "@/strings/SignupStrings";

export default function SignUp() {
  const handleSignup = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form     = e.currentTarget;
    const formData = new FormData(form);

    const firstname       = formData.get("firstname")
    const lastname        = formData.get("lastname")
    const email           = formData.get("email");
    const password        = formData.get("password");
    const confirmPassword = formData.get("confirmPassword");

    // TODO: validate & call backend
  };

  return (
    <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-sm">
        <img
          className="mx-auto h-10 w-auto"
          src="https://tailwindcss.com/plus-assets/img/logos/mark.svg"
          alt="Your Company"
        />
        <h2 className="mt-10 text-center text-2xl font-bold text-gray-900">
          {SignupStrings.Title}
        </h2>
      </div>

      <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
        <form className="space-y-6" onSubmit={handleSignup}>
          <div className="flex gap-5">
            <div>
                <label className="block text-sm font-medium text-primary">
                    {SignupStrings.FirstName}
                </label>
                <input 
                    name="firstname" 
                    type="text" 
                    required
                    className="mt-2 block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 border border-gray-300 placeholder:text-gray-400 focus:outline-indigo-600">
                </input>
            </div>
            <div>
                <label className="block text-sm font-medium text-primary">
                    {SignupStrings.LastName}
                </label>
                <input 
                    name="lastname" 
                    type="text" 
                    required
                    className="mt-2 block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 border border-gray-300 placeholder:text-gray-400 focus:outline-indigo-600">
                </input>
            </div>
          </div>
          <div>
            <label className="block text-sm font-medium text-primary">
              {SignupStrings.EmailLabel}
            </label>
            <input
              name="email"
              type="email"
              required
              className="mt-2 block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 border border-gray-300 placeholder:text-gray-400 focus:outline-indigo-600"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-primary">
              {SignupStrings.PasswordLabel}
            </label>
            <input
              name="password"
              type="password"
              required
              className="mt-2 block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 border border-gray-300 placeholder:text-gray-400 focus:outline-indigo-600"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-primary">
              {SignupStrings.ConfirmPasswordLabel}
            </label>
            <input
              name="confirmPassword"
              type="password"
              required
              className="mt-2 block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 border border-gray-300 placeholder:text-gray-400 focus:outline-indigo-600"
            />
          </div>

          <Button 
            type="submit" 
            className="w-full">
              {SignupStrings.SubmitButton}
          </Button>

        </form>
        
        <p className="mt-10 text-center text-sm/6 text-primary">
            {SignupStrings.AlreadyAMember} 
            <Link 
              to='/login' 
              className="font-semibold text-primary hover:text-[#fcda00] hover:underline ml-1">
                {SignupStrings.Login}
            </Link>
        </p>
        
      </div>
    </div>
  );
}
