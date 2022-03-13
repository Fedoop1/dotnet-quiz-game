import { TOUCH_BUFFER_MS } from '@angular/cdk/a11y/input-modality/input-modality-detector';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { from, Observable, of, Subscription } from 'rxjs';
import {
  defaultIfEmpty,
  filter,
  map,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs/operators';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import { QuizData } from 'src/app/models/quiz-data.model';
import { QuizPlayer } from 'src/app/models/quiz-player.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';

@Component({
  selector: 'session-lobby',
  templateUrl: 'session-lobby.component.html',
  styleUrls: ['session-lobby.component.scss'],
})
export class SessionLobbyComponent
  extends DestroyableComponent
  implements OnInit
{
  public isHost!: boolean;
  public currentPlayer!: QuizPlayer;

  public sessionPlayers: QuizPlayer[] = [];

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {
    super();
  }

  ngOnInit(): void {
    this.isHost = history.state.isHost || false;

    this.quizService
      .connectToHub(this.route.snapshot.queryParams?.nickName, this.isHost)
      .pipe(
        takeUntil(this.onDestroy$),
        switchMap(() => this.loadData()),
        tap(() => this.setupSubscribers())
      )
      .subscribe();
  }

  public leaveLobby() {
    this.quizService.disconnectFromHub();

    return this.isHost
      ? this.router.navigate([''])
      : this.router.navigate(['join-session']);
  }

  public startQuiz() {
    this.quizService
      .startGame(this.route.snapshot.queryParams?.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        tap(() => {
          this.router.navigate(['quiz-host'], {
            state: { quiz: this.getQuizData() },
          });
        })
      )
      .subscribe();
  }

  private loadData(): Observable<QuizPlayer[]> {
    if (this.isHost) return of([]);

    return this.quizService
      .getPlayerInfo(
        this.route.snapshot.queryParams?.sessionId,
        this.route.snapshot.queryParams?.nickName
      )
      .pipe(
        tap((playerInfo) => {
          this.currentPlayer = playerInfo;
          this.sessionPlayers.unshift(this.currentPlayer);
        }),
        switchMap(() => this.updateUsersTable())
      );
  }

  private updateUsersTable() {
    return this.quizService
      .getSessionPlayers(this.route.snapshot.queryParams?.sessionId)
      .pipe(tap((sessionPlayers) => (this.sessionPlayers = sessionPlayers)));
  }

  private setupSubscribers() {
    [this.quizService.playerAdded$, this.quizService.playerRemoved$].forEach(
      (observable) =>
        observable
          .pipe(
            takeUntil(this.onDestroy$),
            switchMap(() => this.updateUsersTable())
          )
          .subscribe()
    );

    this.quizService.sessionState$
      .pipe(
        takeUntil(this.onDestroy$),
        filter((sessionState) => sessionState === SessionState.Closed),
        tap(() =>
          this.router.navigate(['join-session'], {
            queryParams: { sessionClosed: true },
          })
        )
      )
      .subscribe();

    this.quizService.sessionState$
      .pipe(
        takeUntil(this.onDestroy$),
        filter((sessionState) => sessionState === SessionState.Round),
        tap(() => {
          this.router.navigate(['quiz'], {
            state: { quiz: this.getQuizData() },
          });
        })
      )
      .subscribe();
  }

  private getQuizData(): QuizData {
    return {
      player: this.currentPlayer,
      isHost: this.isHost,
      sessionId: this.route.snapshot.queryParams?.sessionId,
    };
  }
}
