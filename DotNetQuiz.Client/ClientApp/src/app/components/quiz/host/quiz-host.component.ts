import { registerLocaleData } from '@angular/common';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, switchMap, takeUntil, tap } from 'rxjs/operators';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import { QuizData } from 'src/app/models/quiz-data.model';
import { QuizPlayer } from 'src/app/models/quiz-player.model';
import { Question } from 'src/app/models/quiz-question.model';
import { QuizRound } from 'src/app/models/quiz-round.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';
import { LeaderBoardComponent } from '../leader-board/leader-board.component';
import { RoundStatisticComponent } from '../round-statistic/round-statistic.component';
import { RoundTimerComponent } from '../round-timer/round-timer.component';
import { DateFormat } from './models/date-format.enum';

@Component({
  selector: 'quiz-host',
  templateUrl: 'quiz-host.component.html',
  styleUrls: ['quiz-host.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class QuizHostComponent extends DestroyableComponent implements OnInit {
  public sessionState: SessionState = SessionState.Idle;

  public quizData!: QuizData;
  public currentRound!: QuizRound;
  public quizQuestions!: Question[];

  public DateFormat = DateFormat;
  public isFirstRound = true;

  public get isRoundButtonsDisabled(): boolean {
    return !this.quizQuestions || !this.quizQuestions.length;
  }

  @ViewChild(RoundTimerComponent) timer?: RoundTimerComponent;
  @ViewChild(LeaderBoardComponent) leaderBoard?: LeaderBoardComponent;
  @ViewChild(RoundStatisticComponent) roundStatistic?: RoundStatisticComponent;

  SessionState = SessionState;

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.quizData = history.state.quiz;

    if (!this.quizData) this.router.navigate(['']);

    this.setupSubscribers();
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

  // TODO: Mark question as completed after next round click instead of removing from the array
  public nextRound() {
    this.quizQuestions.splice(
      this.quizQuestions.findIndex(
        (question) =>
          question.questionId === this.currentRound.question.questionId
      ),
      1
    );

    this.quizService
      .nextRound(this.quizData.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        finalize(() => (this.sessionState = SessionState.Idle)),
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
          this.sessionState = SessionState.Round;

          const startAt = new Date(roundTime.startAt);
          const endAt = new Date(roundTime.endAt);

          this.currentRound.startAt = startAt;
          this.currentRound.endAt = endAt;

          setTimeout(() => {
            this.timer?.startTimer();
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
        tap(() => {
          this.sessionState = SessionState.RoundStatistic;
          this.timer?.stopTimer();
        })
      )
      .subscribe();
  }

  public showLeaderBoard() {
    this.quizService
      .changeSessionState(this.quizData.sessionId, SessionState.LeaderBoard)
      .pipe(
        takeUntil(this.onDestroy$),
        tap(() => {
          this.sessionState = SessionState.LeaderBoard;
          this.timer?.stopTimer();
        })
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

  private setupSubscribers() {
    this.quizService.processAnswer$
      .pipe(
        takeUntil(this.onDestroy$),
        tap(() => {
          this.leaderBoard?.updateData();
          this.roundStatistic?.updateData();
        })
      )
      .subscribe();
  }
}
