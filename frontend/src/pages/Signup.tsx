import { Button } from "@/components/ui/button";
import { useNavigate, Link } from "react-router-dom"
import { useState } from "react";
import SignupStrings from "@/constants/strings/SignupStrings";
import { signup } from "../apis/auth/authService";
import IntConstants from "@/constants/ints/IntConstants";
import { InputError, BannerError } from "@/components/ErrorUI";

function getAxiosErrorMessage(err: unknown) {
  const data = err as { error?: string; message?: string };
  return data?.error || data?.message || "Request failed.";
}

export default function SignUp() {

  const navigate = useNavigate()

  const [submitting, setSubmitting] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [emailError, setEmailError] = useState<string | null>(null);

  const handleSignup = async (e: React.FormEvent<HTMLFormElement>) => {

      e.preventDefault();
      setErrorMessage(null)

      const form     = e.currentTarget;
      const formData = new FormData(form);

      const firstname       = String(formData.get("firstname") || "").trim()
      const lastname        = String(formData.get("lastname") || "").trim()
      const email           = String(formData.get("email") || "").toLowerCase().trim();
      const password        = String(formData.get("password") || "")
      const confirmPassword = String(formData.get("confirmPassword") || "")

      if (!firstname || !lastname || !email || !password){
          alert(SignupStrings.RequiredFields);
          return;
      }

      if (password !== confirmPassword){
          alert(SignupStrings.PasswordsNotSame);
          return;
      }

      try {
          setSubmitting(true);
          await signup({ firstname, lastname, password, email });
          alert(SignupStrings.SignUpConfirmation);
          navigate('/');
      } 
      catch (err) {

          const msg = getAxiosErrorMessage(err);
          if (msg === SignupStrings.DuplicateEmailKey) setEmailError(SignupStrings.DuplicateEmailKey);
          else setErrorMessage(msg)
      } 
      finally {
          setSubmitting(false)
      }
  };

  return (
    <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-sm">
        <img
          className="mx-auto h-10 w-auto"
          src="https://tailwindcss.com/plus-assets/img/logos/mark.svg"
          alt="Your Company"
        />
        <h2 className="mt-10 text-center text-2xl font-bold text-gray-900 pb-2">
          {SignupStrings.Title}
        </h2>
        <BannerError message={errorMessage}/>
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
                    maxLength={IntConstants.maxFirstNameLength}
                    pattern="[A-Za-z]{1,32}"
                    title={SignupStrings.LettersOnly}
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
                    maxLength={IntConstants.maxLastNameLength}
                    pattern="[A-Za-z]{1,32}"
                    title={SignupStrings.LettersOnly}
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
              maxLength={IntConstants.maxEmailLength}
              className="mt-2 block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 border border-gray-300 placeholder:text-gray-400 focus:outline-indigo-600"
              onChange={() => setEmailError(null)}
            />
            <InputError message={emailError}/>
          </div>

          <div>
            <label className="block text-sm font-medium text-primary">
              {SignupStrings.PasswordLabel}
            </label>
            <input
              name="password"
              type="password"
              required
              minLength={IntConstants.minPasswordLength}
              maxLength={IntConstants.maxPasswordLength}
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
              minLength={IntConstants.minPasswordLength}
              maxLength={IntConstants.maxPasswordLength}
              className="mt-2 block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 border border-gray-300 placeholder:text-gray-400 focus:outline-indigo-600"
            />
          </div>

          <Button 
            type="submit" 
            className="w-full"
            disabled={submitting}>
              {submitting ? SignupStrings.Submitting : SignupStrings.SubmitButton}
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
