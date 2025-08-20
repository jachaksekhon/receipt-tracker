export const ResponseStrings = {

    MissingFields : "Missing required fields",
    DuplicateEmail: "An account with that e-mail already exists!",
    EmailNotFound: "Email not found",
    WrongPass: "Incorrect password entered",

    LoginSuccess: "Login successful",
    UserSuccess: "User created successfully!",

    LoginError: "Error logging in",
    SignupError: "Signup Error: ",
    InternalError: "Internal server error",
    GettingUsersError: "Error getting users",

    JWTNotDefined: "JWT_SECRET is not defined",
    KeyExpiry: "1d"

} as const