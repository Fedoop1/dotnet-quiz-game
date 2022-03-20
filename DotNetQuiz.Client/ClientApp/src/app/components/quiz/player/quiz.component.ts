import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Router } from '@angular/router';
import { takeUntil, tap } from 'rxjs/operators';
import { QuestionType } from 'src/app/models/enums/question-type.enum';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import { QuizData } from 'src/app/models/quiz-data.model';
import { QuizPlayerAnswer } from 'src/app/models/quiz-player-answer.model';
import { QuizRound } from 'src/app/models/quiz-round.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';
import { QuestionComponent } from '../question/question.component';
import { RoundTimerComponent } from '../round-timer/round-timer.component';

@Component({
  selector: 'quiz',
  templateUrl: 'quiz.component.html',
  styleUrls: ['quiz.component.scss'],
})
export class QuizComponent extends DestroyableComponent implements OnInit {
  private _sessionState = SessionState.Idle;

  public quizData!: QuizData;
  public quizRound!: QuizRound;
  public isButtonDisabled: boolean = false;

  public QuestionType = QuestionType;
  public SessionState = SessionState;

  public get sessionState(): SessionState {
    return this._sessionState;
  }

  public set sessionState(sessionState: SessionState) {
    this._sessionState = sessionState;
  }

  @ViewChild(RoundTimerComponent) timer?: RoundTimerComponent;
  @ViewChild(QuestionComponent) question?: QuestionComponent;

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {
    super();
  }

  public submitAnswer() {
    return (answer: string) => {
      const questionAnswer: QuizPlayerAnswer = {
        playerId: this.quizData.player.id,
        answerContent: answer,
        answerTime: Date.now() - this.quizRound.startAt!.getTime(),
      };

      this.quizService
        .submitAnswer(questionAnswer, this.quizData.sessionId)
        .pipe(
          takeUntil(this.onDestroy$),
          tap(() => (this.isButtonDisabled = true))
        )
        .subscribe();
    };
  }

  ngOnInit(): void {
    this.quizData = history.state.quiz;

    if (!this.quizData) this.router.navigate(['join-session']);

    this.setupSubscribers();
  }

  private setupSubscribers() {
    this.quizService.sessionState$
      .pipe(
        takeUntil(this.onDestroy$),
        tap((sessionState) => {
          this.sessionState = sessionState;
          this.processSessionStateChange();
        }),

        tap(() => this.cdr.markForCheck())
      )
      .subscribe();
  }

  private processSessionStateChange() {
    switch (this.sessionState) {
      case SessionState.Closed:
        this.router.navigate(['join-session'], {
          queryParams: { sessionClosed: true },
        });
        return;
      case SessionState.Round:
        this.quizService
          .getQuizRound(this.quizData.sessionId)
          .pipe(
            takeUntil(this.onDestroy$),
            tap((quizRound) => {
              this.quizRound = quizRound;
              this.isButtonDisabled = false;

              setTimeout(() => {
                this.question?.displayQuestion();
                this.timer?.startTimer();
              });
            })
          )
          .subscribe();
        return;
    }
  }
}
