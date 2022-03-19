import { registerLocaleData } from '@angular/common';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import { QuizData } from 'src/app/models/quiz-data.model';
import { QuizPlayer } from 'src/app/models/quiz-player.model';
import { Question } from 'src/app/models/quiz-question.model';
import { QuizRound } from 'src/app/models/quiz-round.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';
import { RoundTimerComponent } from '../round-timer/round-timer.component';
import { DateFormat } from './models/date-format.enum';

@Component({
  selector: 'quiz-host',
  templateUrl: 'quiz-host.component.html',
  styleUrls: ['quiz-host.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class QuizHostComponent extends DestroyableComponent implements OnInit {
  public quizData!: QuizData;
  public currentRound!: QuizRound;
  public quizQuestions!: Question[];

  public DateFormat = DateFormat;
  public isFirstRound = true;

  public get isRoundButtonsDisabled(): boolean {
    return (
      !this.quizQuestions ||
      !this.quizQuestions.length ||
      (this.quizQuestions && this.quizQuestions?.length === 1)
    );
  }

  @ViewChild(RoundTimerComponent) timer!: RoundTimerComponent;

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.quizData = history.state.quiz;

    if (!this.quizData) this.router.navigate(['']);

    this.loadData();
  }

  private loadData() {
    this.loadRound()
      .pipe(
        switchMap(() =>
          this.quizService.getQuizQuestions(this.quizData.sessionId)
        ),
        tap((quizQuestions) => (this.quizQuestions = quizQuestions))
      )
      .subscribe();
  }

  public nextRound() {
    this.quizQuestions.splice(
      this.quizQuestions.findIndex(
        (question) => question.questionId === this.currentRound.questionId
      ),
      1
    );

    this.quizService
      .nextRound(this.quizData.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        switchMap(() => this.loadRound())
      )
      .subscribe();
  }

  public startRound() {
    this.quizService
      .startRound(this.quizData.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        tap((roundTime) => {
          const startAt = new Date(roundTime.startAt);
          const endAt = new Date(roundTime.endAt);

          this.currentRound.startAt = startAt;
          this.currentRound.endAt = endAt;

          setTimeout(() => {
            this.timer.startTimer();
          });
        })
      )
      .subscribe();
  }

  public showStatistic() {
    this.quizService
      .changeSessionState(this.quizData.sessionId, SessionState.RoundStatistic)
      .pipe(
        takeUntil(this.onDestroy$),
        tap(() => this.timer.stopTimer())
      )
      .subscribe();
  }

  public showLeaderBoard() {
    this.quizService
      .changeSessionState(this.quizData.sessionId, SessionState.LeaderBoard)
      .pipe(
        takeUntil(this.onDestroy$),
        tap(() => this.timer.stopTimer())
      )
      .subscribe();
  }

  public closeSession() {
    this.quizService.disconnectFromHub();
    this.router.navigate(['']);
  }

  private loadRound() {
    return this.quizService.getQuizRound(this.quizData.sessionId).pipe(
      takeUntil(this.onDestroy$),
      tap((round) => (this.currentRound = round))
    );
  }
}
