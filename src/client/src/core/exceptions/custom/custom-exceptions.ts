import { ErrorModel } from "../../models/error/error.model";


export class NotFoundException extends ErrorModel {
  constructor(code = '404',message = 'Page not found!') {
    super(code, message);
  }
}

export class ForbiddenException  extends ErrorModel {
  constructor(code = '403',message = 'Access is forbidden!') {
    super(code, message);
  }
}

export class UnauthorizedException  extends ErrorModel {
  constructor(code = '401',message = 'You have no permission to access!') {
    super(code, message);
  }
}

export class InternalServerErrorException extends ErrorModel {
  constructor(code = '500',message = 'Server error, please try again later!') {
    super(code, message);
  }
}
