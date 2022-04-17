import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, timer } from 'rxjs';
import { filter, first, switchMap, timeout } from 'rxjs/operators';
import { ErrorNotification } from '../models/error-notification.model';

@Injectable()
export class ErrorNotificationService {
  private _notification$ = new BehaviorSubject<ErrorNotification | null>(null);

  public readonly notification$: Observable<ErrorNotification | null> =
    this._notification$.asObservable();

  public showErrorNotification(errorNotification: ErrorNotification | null) {
    this._notification$.next(errorNotification);
  }

  public clear() {
    this._notification$.next(null);
  }

  constructor() {
    this.notification$
      .pipe(
        filter((notification) => !!notification),
        switchMap(() => timer(10000).pipe(first()))
      )
      .subscribe(() => this.clear());
  }
}
