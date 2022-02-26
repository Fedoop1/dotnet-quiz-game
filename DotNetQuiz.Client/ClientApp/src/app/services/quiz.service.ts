import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import AppConfiguration from '../models/constants/configuraion';
import { QuizConfiguration } from '../models/quiz-configuration.model';

@Injectable()
export class QuizService {
  constructor(private readonly httpClient: HttpClient) {}

  public createQuizSession(): Observable<string> {
    return this.httpClient.post<string>(
      `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/Create`,
      undefined,
      undefined
    );
  }

  public configureQuizSession(
    sessionId: string,
    quizConfiguration: QuizConfiguration
  ): Observable<void> {
    return this.httpClient.post<void>(
      `${AppConfiguration.BackendServerAddress}/${AppConfiguration.QuizControllerAddress}/Configure`,
      { sessionId: sessionId, quizConfiguration: quizConfiguration },
      undefined
    );
  }
}
