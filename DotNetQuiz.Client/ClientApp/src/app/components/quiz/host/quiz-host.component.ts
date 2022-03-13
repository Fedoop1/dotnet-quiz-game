import { registerLocaleData } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import { QuizData } from 'src/app/models/quiz-data.model';
import { QuizPlayer } from 'src/app/models/quiz-player.model';
import { QuizRound } from 'src/app/models/quiz-round.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';
import { RoundTimerComponent } from '../round-timer/round-timer.component';
import { DateFormat } from './models/date-format.enum';

@Component({
  selector: 'quiz-host',
  templateUrl: 'quiz-host.component.html',
  styleUrls: ['quiz-host.component.scss'],
})
// TODO: Add timeout which track question time
export class QuizHostComponent extends DestroyableComponent implements OnInit {
  public quizData!: QuizData;
  public currentRound!: QuizRound;

  public DateFormat = DateFormat;
  public isFirstRound = true;

  @ViewChild('timer') timer!: RoundTimerComponent;

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.quizData = history.state.quiz;

    // if (!this.quizData) this.router.navigate(['']);

    this.loadData();
  }

  private loadData() {
    this.loadRound().subscribe();
  }

  public nextRound() {
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
        tap(() => this.timer.startTimer())
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

  private loadRound() {
    return this.quizService.getQuizRound(this.quizData.sessionId).pipe(
      takeUntil(this.onDestroy$),
      tap((round) => (this.currentRound = round))
    );
  }
}
