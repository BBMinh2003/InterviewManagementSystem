export class ErrorModel {
  public code!: string;
  public message!: string;

  constructor(code: string, message: string) {
    this.code = code;
    this.message = message;
  }
}
