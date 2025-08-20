
import LoginStrings from "@/constants/strings/LoginStrings";

type InputErrorProps = {
    message? : string | null;
}

export function InputError( {message} : InputErrorProps ) {
    if (!message) return null;
    return (
        <p className="mt-1 text-xs text-red-600" role="alert">
            {message}
        </p>
    );
}

type BannerErrorProps = {
    message? : string | null;
}

export function BannerError( {message} : BannerErrorProps) {
    if (!message) return null;
    return (
    <div className="pt-2 p-3 mb-3 text-sm text-red-800 rounded-lg bg-red-50 break-words text-center" role="alert">
      <span className="font-medium">{LoginStrings.Error}</span> {message}
    </div>
  );
}