import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Router,
} from '@angular/router';
import { NEVER } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DefaultQuestionPack } from 'src/app/models/constants/default-question-pack';
import { QuizConfiguration } from 'src/app/models/quiz-configuration.model';
import { QuizService } from 'src/app/services/quiz.service';

@Component({
  selector: 'create-session',
  templateUrl: 'create-session.component.html',
  styleUrls: ['create-session.component.scss'],
})
export class CreateSessionComponent {
  public quizConfiguration: QuizConfiguration = {
    roundDuration: 60,
    maxPlayers: 4,
    streakMultiplier: 1,
    timeMultiplier: 1,
    answerIgnoreCase: false,
  } as QuizConfiguration;

  public isShowValidationError: boolean = false;
  public errorMessage?: string;

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {}

  public onCreateSessionClick() {
    this.isShowValidationError = false;

    if (!this.validateQuizConfiguration()) {
      this.isShowValidationError = true;
      return;
    }

    this.quizService
      .configureQuizSession(
        this.route.snapshot.queryParams?.sessionId,
        this.quizConfiguration
      )
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.errorMessage = error.message;
          return NEVER;
        })
      );
  }

  public onBackButtonClick() {
    this.router.navigate(['home']);
  }

  private validateQuizConfiguration(): boolean {
    return (
      this.quizConfiguration.roundDuration >= 5 &&
      this.quizConfiguration.maxPlayers > 0 &&
      this.quizConfiguration.streakMultiplier >= 1 &&
      this.quizConfiguration.timeMultiplier >= 1 &&
      this.validateQuestionPack()
    );
  }

  private validateQuestionPack(): boolean {
    return (
      this.quizConfiguration.questionPack.questions &&
      this.quizConfiguration.questionPack.questions.every((question) => {
        question.questionId &&
          question.questionReward >= 0 &&
          question.content &&
          question.answer;
      })
    );
  }
}
