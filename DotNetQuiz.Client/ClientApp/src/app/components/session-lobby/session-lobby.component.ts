import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { from, Subscription } from 'rxjs';
import { filter, switchMap, takeUntil, tap } from 'rxjs/operators';
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
  private onDestroySubscription!: Subscription;

  public isHost!: boolean;
  public currentPlayer!: QuizPlayer;

  public sessionPlayers: QuizPlayer[] = [{ id: 1, nickName: 'testUser' }];

  constructor(
    private readonly quizService: QuizService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.isHost = history.state.isHost || false;
    this.currentPlayer = history.state.player;

    this.loadData();
    this.setupSubscribers();
  }

  public leaveLobby(isKicked: boolean) {
    this.onDestroySubscription.unsubscribe();

    if (this.isHost) {
      this.closeQuizSession();
      return;
    }

    this.quizService
      .removePlayer(
        this.currentPlayer.id,
        this.route.snapshot.queryParams?.sessionId
      )
      .pipe(
        tap(() =>
          this.router.navigate(['join-session'], {
            state: { isKicked: isKicked },
          })
        )
      )
      .subscribe();
  }

  public startQuiz() {}

  public removePlayer(playerId: number) {
    this.quizService
      .removePlayer(playerId, this.route.snapshot.queryParams?.sessionId, true)
      .subscribe();
  }

  private loadData() {
    this.updateUsersTable().subscribe();
  }

  private updateUsersTable() {
    return this.quizService
      .loadSessionPlayers(this.route.snapshot.queryParams?.sessionId)
      .pipe(tap((sessionPlayers) => (this.sessionPlayers = sessionPlayers)));
  }

  private closeQuizSession() {
    this.quizService
      .closeQuizSession(this.route.snapshot.queryParams?.sessionId)
      .pipe(tap(() => this.router.navigate([''])))
      .subscribe();
  }

  private setupSubscribers() {
    from([this.quizService.playerAdded$, this.quizService.playerRemoved$])
      .pipe(
        takeUntil(this.onDestroy$),
        switchMap(() => this.updateUsersTable())
      )
      .subscribe();

    this.quizService.userConnectionClosed$
      .pipe(
        filter((userId) => userId === this.currentPlayer.id),
        tap(() => this.leaveLobby(true))
      )
      .subscribe();

    this.onDestroySubscription = this.onDestroy$.subscribe(() =>
      this.isHost ? this.closeQuizSession() : this.leaveLobby(false)
    );
  }
}
