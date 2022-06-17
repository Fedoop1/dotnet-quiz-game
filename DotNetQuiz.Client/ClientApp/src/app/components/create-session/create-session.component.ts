import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Router,
} from '@angular/router';
import { NEVER } from 'rxjs';
import { catchError, filter, finalize, tap } from 'rxjs/operators';
import { QuizConfiguration } from 'src/app/models/quiz-configuration.model';
import { QuizConfigurationService } from 'src/app/services/quiz-configuration.service';
import { QuizService } from 'src/app/services/quiz.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { PackBuilderComponent } from '../pack-builder/pack-builder.component';
import { QuestionPack } from 'src/app/models/quiz-question-pack.model';

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

  public get isQuestionPackSelected(): boolean {
    return !!this.quizConfiguration.questionPack;
  }

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly dialog: MatDialog,
    private readonly quizConfigurationService: QuizConfigurationService
  ) {}

  public onQuestionPackChange(event: any) {
    if (event.target.files.length === 0) {
      this.quizConfiguration.questionPack = undefined;
      return;
    }

    this.quizConfigurationService
      .parseQuestionPack(event.target.files[0])
      .pipe(
        tap(
          (questionPack) => (this.quizConfiguration.questionPack = questionPack)
        )
      )
      .subscribe();
  }

  public downloadClick() {
    const anchor = document.createElement('a');
    const encodedPack = encodeURIComponent(
      JSON.stringify(this.quizConfiguration.questionPack)
    );

    anchor.setAttribute('href', 'data:text/json;charset=utf-8,' + encodedPack);
    anchor.setAttribute('download', 'question_pack.json');
    anchor.click();
  }

  public onQuestionPackClick() {
    const modalRef = this.dialog.open<
      PackBuilderComponent,
      QuestionPack | undefined,
      QuestionPack | undefined
    >(PackBuilderComponent, {
      data: this.quizConfiguration.questionPack,
      width: '500px',
      minHeight: 500,
      maxHeight: 840,
      disableClose: true,
    });

    modalRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        tap((result) => (this.quizConfiguration.questionPack = result))
      )
      .subscribe();
  }

  public createSession() {
    this.isShowValidationError = false;

    if (
      !this.quizConfigurationService.validateQuizConfiguration(
        this.quizConfiguration
      )
    ) {
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
        }),
        tap(() =>
          this.router.navigate(['session-lobby'], {
            queryParams: {
              sessionId: this.route.snapshot.queryParams?.sessionId,
            },
            state: { isHost: true },
          })
        )
      )
      .subscribe();
  }

  public Back() {
    this.quizService
      .removeQuizSession(this.route.snapshot.queryParams?.sessionId)
      .pipe(finalize(() => this.router.navigate(['home'])))
      .subscribe();
  }
}
