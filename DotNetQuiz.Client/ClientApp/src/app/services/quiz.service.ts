import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NEVER, Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { QuizSession } from '../components/join-session/models/quiz-session.model';
import AppConfiguration from '../models/constants/configuraion';
import { QuizConfiguration } from '../models/quiz-configuration.model';

@Injectable()
export class QuizService {
  constructor(private readonly httpClient: HttpClient) {}

  public createQuizSession(): Observable<string> {
    return this.httpClient
      .post<string>(
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/Create`,
        undefined,
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
        `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/Configure`,
        { sessionId: sessionId, quizConfiguration: quizConfiguration },
        undefined
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

  private catchResponseError(error: HttpErrorResponse) {
    console.error('Backend return error:', error);
    return throwError(() => error);
  }
}
