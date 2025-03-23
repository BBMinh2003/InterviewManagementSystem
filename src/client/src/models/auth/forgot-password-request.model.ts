export class ForgotPasswordRequest {
  public email: string;
  public clientURI: string;

  constructor(email: string, clientURI: string) {
    this.email = email;
    this.clientURI = clientURI;
  }
}
