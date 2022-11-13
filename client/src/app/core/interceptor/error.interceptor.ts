import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { statusCodes } from 'src/app/shared/models/statusCodes';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toast: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          const errorInErrorObject = error.error;
          const errorList = error.error.errors;
          if (error.status === statusCodes.badRequest) {
            if (errorList) {
              throw errorInErrorObject;
            }
            else {
              this.toast.error(error.error.message, error.error.statusCode);
            }
          }
          if (error.status === statusCodes.unAuthorized) {
            this.toast.error(error.error.message, error.error.statusCode);
          }
          if (error.status === statusCodes.notFound) {
            this.toast.error(error.error.message, error.error.statusCode);
            this.router.navigateByUrl('/not-found');
          }

          if (error.status === statusCodes.internalServer) {
            const navigationExtras: NavigationExtras = { state: { error: errorInErrorObject } }
            this.toast.error(error.error.message, error.error.statusCode);
            this.router.navigateByUrl('/server-error', navigationExtras);
          }
        }
        return throwError(error);
      })
    );
  }
}
