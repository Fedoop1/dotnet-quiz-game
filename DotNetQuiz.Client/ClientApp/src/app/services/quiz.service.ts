import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { from, NEVER, Observable, of, Subject, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { QuizSession } from '../components/join-session/models/quiz-session.model';
import AppConfiguration from '../models/constants/configuraion';
import { QuizConfiguration } from '../models/quiz-configuration.model';
import { QuizPlayer } from '../models/quiz-player.model';
import { DestroyableComponent } from '../utils/destroyable-component/destroyable.component';

@Injectable()
export class QuizService implements OnDestroy {
  private quizHubConnection?: HubConnection;

  private _playerAdded$: Subject<QuizPlayer> = new Subject();
  private _playerRemoved$: Subject<QuizPlayer> = new Subject();
  private _sessionClosed$: Subject<void> = new Subject();

  public playerAdded$ = this._playerAdded$.asObservable();
  public playerRemoved$ = this._playerRemoved$.asObservable();
  public sessionClosed$ = this._sessionClosed$.asObservable();

  constructor(
    private readonly httpClient: HttpClient,
    private readonly route: ActivatedRoute
  ) {}

  ngOnDestroy(): void {
    this.quizHubConnection?.stop();
  }

  public createQuizSession(): Observable<string> {
    return this.httpClient
      .post<string>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/Create`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public RemoveQuizSession(sessionId: string): Observable<string> {
    return this.httpClient
      .post<string>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/Remove`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public configureQuizSession(
    sessionId: string,
    quizConfiguration: QuizConfiguration
  ): Observable<void> {
    return this.httpClient
      .post<void>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/Configure/`,
        quizConfiguration,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getPlayerInfo(sessionId: string, nickName: string) {
    return this.httpClient
      .get<QuizPlayer>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/GetPlayerInfo/${nickName}`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getSessionPlayers(sessionId: string): Observable<QuizPlayer[]> {
    return this.httpClient
      .get<QuizPlayer[]>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/GetSessionPlayers/`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getQuizSessions(): Observable<QuizSession[]> {
    return this.httpClient
      .get<QuizSession[]>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/GetQuizSessions`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public connectToHub(userName: string, isHost: boolean = false) {
    if (this.quizHubConnection) this.disconnectFromHub();

    this.quizHubConnection = new HubConnectionBuilder()
      .withUrl(
        `${AppConfiguration.BackendServerAddress}/quiz/${
          this.route.snapshot.queryParams?.sessionId
        }/${isHost ? 'host' : userName}/${isHost}`
      )
      .build();

    return from(this.quizHubConnection.start()).pipe(
      catchError(this.catchResponseError),
      tap(() => this.setupSubscribers())
    );
  }

  public disconnectFromHub() {
    this.quizHubConnection?.stop();
    this.quizHubConnection = undefined;
  }

  private setupSubscribers() {
    this.quizHubConnection?.on('playerAdded', (quizPlayer: QuizPlayer) => {
      this._playerAdded$.next(quizPlayer);
    });

    this.quizHubConnection?.on('playerRemoved', (quizPlayer: QuizPlayer) => {
      this._playerRemoved$.next(quizPlayer);
    });

    this.quizHubConnection?.on('sessionClosed', () => {
      this._sessionClosed$.next(void 0);
      this.disconnectFromHub();
    });
  }

  private catchResponseError(error: HttpErrorResponse) {
    console.error('Backend return error:', error);
    return throwError(() => error);
  }
}
