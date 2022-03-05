import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { NEVER, Observable, of, Subject, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { QuizSession } from '../components/join-session/models/quiz-session.model';
import AppConfiguration from '../models/constants/configuraion';
import { QuizConfiguration } from '../models/quiz-configuration.model';
import { QuizPlayer } from '../models/quiz-player.model';
import { DestroyableComponent } from '../utils/destroyable-component/destroyable.component';

@Injectable()
export class QuizService implements OnDestroy {
  private quizHubConnection!: HubConnection;

  private _playerAdded$: Subject<void> = new Subject();
  private _playerRemoved$: Subject<void> = new Subject();
  private _connectionClose$: Subject<string> = new Subject();
  private _userConnectionClosed$: Subject<number> = new Subject();

  public playerAdded$ = this._playerAdded$.asObservable();
  public playerRemoved$ = this._playerRemoved$.asObservable();
  public userConnectionClosed$ = this._userConnectionClosed$.asObservable();
  public connectionClose$ = this._connectionClose$.asObservable();

  constructor(
    private readonly httpClient: HttpClient,
    private readonly route: ActivatedRoute
  ) {
    this.buildConnection();
    this.setupSubscribers();
  }

  ngOnDestroy(): void {
    this.quizHubConnection.stop();
  }

  public removePlayer(
    playerId: number,
    sessionId: string,
    force: boolean = false
  ) {
    return this.httpClient
      .post(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/RemovePlayer/${playerId}`,
        null,
        { params: { force: force } }
      )
      .pipe(catchError(this.catchResponseError));
  }

  public createQuizSession(): Observable<string> {
    return this.httpClient
      .post<string>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/Create`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public closeQuizSession(sessionId: number): Observable<void> {
    return this.httpClient
      .post<void>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/Close/`,
        null
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

  public loadSessionPlayers(sessionId: string): Observable<QuizPlayer[]> {
    return this.httpClient
      .get<QuizPlayer[]>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/GetSessionPlayers/`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public loadQuizSessions(): Observable<QuizSession[]> {
    return this.httpClient
      .get<QuizSession[]>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/GetQuizSessions`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public addPlayer(
    nickName: string,
    userId: number,
    sessionId: string
  ): Observable<void> {
    return this.httpClient.post<void>(
      `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/${sessionId}/AddPlayer`,
      {
        nickName: nickName,
        id: userId,
      }
    );
  }

  private catchResponseError(error: HttpErrorResponse) {
    console.error('Backend return error:', error);
    return throwError(() => error);
  }

  private buildConnection() {
    this.quizHubConnection = new HubConnectionBuilder()
      .withUrl(
        `${AppConfiguration.BackendServerAddress}/quiz/${this.route.snapshot.queryParams?.sessionId}`
      )
      .build();
  }

  private setupSubscribers() {
    this.quizHubConnection.on('playerAdded', () => {
      this._playerAdded$.next();
    });

    this.quizHubConnection.on('playerRemoved', () => {
      this._playerRemoved$.next();
    });

    this.quizHubConnection.on(
      'userConnectionClosed',
      (user: { userId: number }) => {
        this._userConnectionClosed$.next(user.userId);
      }
    );

    this.quizHubConnection.on('connectionClosed', (message: string[]) => {
      this._connectionClose$.next(
        message ? message[0] : 'Connection was closed'
      );
    });
  }
}
