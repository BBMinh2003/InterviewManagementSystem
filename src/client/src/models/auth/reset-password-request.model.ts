export class ResetPasswordRequest {
    public password: string;
    public confirmPassword: string;
    public email: string;
    public token: string;

    constructor(password: string, confirmPassword: string, email: string, token: string) {
        this.password = password;
        this.confirmPassword = confirmPassword;
        this.email = email;
        this.token = token;
    }
}
