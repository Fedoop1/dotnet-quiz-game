import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { QuizService } from 'src/app/services/quiz.service';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { NEVER } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public errorMessage?: string;

  constructor(
    private readonly router: Router,
    private readonly quizService: QuizService
  ) {}

  public OnCreateSessionClick() {
    this.clearErrorMessage();

    this.quizService
      .createQuizSession()
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.errorMessage = error.message;
          return NEVER;
        })
      )
      .subscribe((sessionId) => {
        this.router.navigate(['create-session'], {
          queryParams: {
            sessionId: sessionId,
          },
        });
      });
  }

  public OnJoinToSessionClick() {
    this.clearErrorMessage();

    // TODO: add join session logic here
    this.router.navigate(['join-session']);
  }

  private clearErrorMessage() {
    this.errorMessage = undefined;
  }
}
