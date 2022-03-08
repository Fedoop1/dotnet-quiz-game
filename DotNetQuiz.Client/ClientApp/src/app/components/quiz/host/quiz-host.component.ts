import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import { QuizData } from 'src/app/models/quiz-data.model';
import { QuizPlayer } from 'src/app/models/quiz-player.model';
import { QuizRound } from 'src/app/models/quiz-round.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';

@Component({
  selector: 'quiz-host',
  templateUrl: 'quiz-host.component.html',
  styleUrls: ['quiz-host.component.scss'],
})
// TODO: Add timeout which track question time
export class QuizHostComponent extends DestroyableComponent implements OnInit {
  public quizData!: QuizData;
  public currentRound!: QuizRound;

  public get startAt(): string {
    return new Date(this.currentRound.startAt).toLocaleTimeString();
  }

  public get endAt(): string {
    return new Date(this.currentRound.endAt).toLocaleTimeString();
  }

  public get timesLeft(): string {
    let leftInMilliseconds =
      this.currentRound.endAt - this.currentRound.startAt;

    leftInMilliseconds = leftInMilliseconds < 0 ? 0 : leftInMilliseconds;

    const secondsLeft = leftInMilliseconds % 60;
    const minutesLeft = leftInMilliseconds / 60;

    return `${minutesLeft}:${secondsLeft}`;
  }

  public get roundProgress(): number {
    return (
      ((this.currentRound.endAt - this.currentRound.startAt / 1000) * 100) / 60
    );
  }

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router
  ) {
    super();

    this.loadData();
  }

  ngOnInit(): void {
    this.quizData = history.state.quiz;

    if (!this.quizData) this.router.navigate(['']);
  }

  private loadData() {
    this.quizService
      .getQuizRound(this.quizData.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        tap((round) => (this.currentRound = round))
      )
      .subscribe();
  }

  public nextRound() {
    this.quizService
      .NextRound(this.quizData.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        switchMap(() =>
          this.quizService.changeSessionState(
            this.quizData.sessionId,
            SessionState.Round
          )
        )
      )
      .subscribe();
  }

  public showStatistic() {
    this.quizService
      .changeSessionState(this.quizData.sessionId, SessionState.RoundStatistic)
      .pipe(takeUntil(this.onDestroy$))
      .subscribe();
  }

  public showLeaderBoard() {
    this.quizService
      .changeSessionState(this.quizData.sessionId, SessionState.LeaderBoard)
      .pipe(takeUntil(this.onDestroy$))
      .subscribe();
  }
}
