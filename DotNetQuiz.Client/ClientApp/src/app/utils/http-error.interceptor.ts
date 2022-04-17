import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NEVER, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorNotificationService } from '../services/error-notification.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(
    private readonly errorNotificationService: ErrorNotificationService
  ) {}

  public intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        this.errorNotificationService.showErrorNotification({
          title: 'Oops... Something went wrong',
          message: error.message,
        });
        return NEVER;
      })
    );
  }
}
