import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { from, Observable, of, Subject, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { QuizSession } from '../components/join-session/models/quiz-session.model';
import { SessionState } from '../models/enums/round-state.enum.model';
import { ErrorNotification } from '../models/error-notification.model';
import { QuizConfiguration } from '../models/quiz-configuration.model';
import { QuizPlayerAnswer } from '../models/quiz-player-answer.model';
import { QuizPlayer } from '../models/quiz-player.model';
import { Question } from '../models/quiz-question.model';
import { QuizRound } from '../models/quiz-round.model';
import { RoundStatistic } from '../models/round-statistic.model';
import { ErrorNotificationService } from './error-notification.service';

@Injectable()
export class QuizService implements OnDestroy {
  private quizHubConnection?: HubConnection;

  private _playerAdded$: Subject<QuizPlayer> = new Subject();
  private _playerRemoved$: Subject<QuizPlayer> = new Subject();
  private _sessionState$: Subject<SessionState> = new Subject();
  private _processAnswer$: Subject<QuizPlayer> = new Subject();

  public playerAdded$ = this._playerAdded$.asObservable();
  public playerRemoved$ = this._playerRemoved$.asObservable();
  public processAnswer$ = this._processAnswer$.asObservable();
  public sessionState$ = this._sessionState$.asObservable();

  constructor(
    private readonly httpClient: HttpClient,
    private readonly route: ActivatedRoute,
    private readonly errorNotificationService: ErrorNotificationService
  ) {}

  ngOnDestroy(): void {
    this.quizHubConnection?.stop();
  }

  public createQuizSession(): Observable<string> {
    return this.httpClient
      .post<string>(
        `${environment.backendServerAddress}/api/quiz/Create`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public startGame(sessionId: string) {
    return this.httpClient
      .post<string>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/StartGame`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public removeQuizSession(sessionId: string): Observable<string> {
    return this.httpClient
      .post<string>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/Remove`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public nextRound(sessionId: string) {
    return this.httpClient
      .post<string>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/NextRound`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public startRound(sessionId: string) {
    return this.httpClient
      .post<{ startAt: string; endAt: string }>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/StartRound`,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public changeSessionState(sessionId: string, sessionState: SessionState) {
    return of(
      this.quizHubConnection?.invoke(
        'changeSessionState',
        sessionId,
        sessionState
      )
    );
  }

  public submitAnswer(answer: QuizPlayerAnswer, sessionId: string) {
    return of(this.quizHubConnection?.send('processAnswer', sessionId, answer));
  }

  public configureQuizSession(
    sessionId: string,
    quizConfiguration: QuizConfiguration
  ): Observable<void> {
    return this.httpClient
      .post<void>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/Configure/`,
        quizConfiguration,
        undefined
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getPlayerInfo(sessionId: string, nickName: string) {
    return this.httpClient
      .get<QuizPlayer>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/GetPlayerInfo/${nickName}/`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getRoundStatistic(sessionId: string): Observable<RoundStatistic> {
    return this.httpClient
      .get<RoundStatistic>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/GetRoundStatistic/`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getQuizRound(sessionId: string): Observable<QuizRound> {
    return this.httpClient
      .get<QuizRound>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/GetQuizRound/`
      )
      .pipe(
        catchError(this.catchResponseError),
        map((quizRound) => {
          if (quizRound.endAt && quizRound.startAt) {
            // * Backend return date in JSON string format
            quizRound.startAt = new Date(quizRound.startAt + '');
            quizRound.endAt = new Date(quizRound.endAt + '');
          }

          return quizRound;
        })
      );
  }

  public getQuizQuestions(sessionId: string): Observable<Question[]> {
    return this.httpClient
      .get<Question[]>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/GetQuizQuestions/`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getSessionPlayers(sessionId: string): Observable<QuizPlayer[]> {
    return this.httpClient
      .get<QuizPlayer[]>(
        `${environment.backendServerAddress}/api/quiz/${sessionId}/GetSessionPlayers/`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public getQuizSessions(): Observable<QuizSession[]> {
    return this.httpClient
      .get<QuizSession[]>(
        `${environment.backendServerAddress}/api/quiz/GetQuizSessions/`
      )
      .pipe(catchError(this.catchResponseError));
  }

  public connectToHub(userName: string, isHost: boolean = false) {
    if (this.quizHubConnection) this.disconnectFromHub();

    this.quizHubConnection = new HubConnectionBuilder()
      .withUrl(
        `${environment.backendServerAddress}/quiz/${
          this.route.snapshot.queryParams?.sessionId
        }/${isHost ? 'host' : userName}/${isHost}/`
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

    this.quizHubConnection?.on(
      'sessionStateChanged',
      (sessionState: SessionState) => {
        this._sessionState$.next(sessionState);
      }
    );

    this.quizHubConnection?.on('processAnswer', (player: QuizPlayer) => {
      this._processAnswer$.next(player);
    });

    this.quizHubConnection?.on('error', (error: ErrorNotification) =>
      this.errorNotificationService.showErrorNotification(error)
    );
  }

  private catchResponseError(error: HttpErrorResponse) {
    console.error('Backend return error:', error);
    return throwError(() => error);
  }
}
