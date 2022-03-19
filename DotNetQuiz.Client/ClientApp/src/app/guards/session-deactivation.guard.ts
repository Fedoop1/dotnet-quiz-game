import { Inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanDeactivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { QuizService } from '../services/quiz.service';

@Injectable()
export class SessionDeactivationGuard implements CanDeactivate<unknown> {
  constructor(private readonly sessionService: QuizService) {}

  public canDeactivate(): boolean {
    this.sessionService.disconnectFromHub();
    return true;
  }
}
