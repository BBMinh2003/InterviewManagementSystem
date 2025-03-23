import { HttpInterceptorFn } from '@angular/common/http';
import { PERMISSION_SERVICE } from '../constants/injection.constant';
import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

export const authInterceptor: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const permissionService = inject(PERMISSION_SERVICE);

  // Check if the request is for the login endpoint
  return permissionService.canActivate();
};
