import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { QuizService } from 'src/app/services/quiz.service';
import { QuizSessionStatus } from './models/quiz-session-status.enum';
import { QuizSession } from './models/quiz-session.model';

@Component({
  selector: 'join-session',
  templateUrl: 'join-session.component.html',
  styleUrls: ['join-session.component.scss'],
})
export class JoinSessionComponent implements OnInit {
  public quizSessions: QuizSession[] = [
    {
      countOfPlayers: 5,
      maxPlayers: 6,
      sessionId: 'Dummy text',
      sessionState: QuizSessionStatus.NotStarted,
    },
    {
      countOfPlayers: 6,
      maxPlayers: 6,
      sessionId: 'Dummy text',
      sessionState: QuizSessionStatus.Running,
    },
    {
      countOfPlayers: 1,
      maxPlayers: 6,
      sessionId: 'Dummy text',
      sessionState: QuizSessionStatus.NotStarted,
    },
  ];

  public QuizSessionStatus = QuizSessionStatus;

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    // this.loadActiveSessions();
  }

  public onRefreshButtonClick() {
    this.loadActiveSessions();
  }

  public onJoinButtonClick(sessionId: string) {
    this.router.navigate(['create-player'], {
      queryParams: { sessionId: sessionId },
    });
  }

  public onHomeButtonClick() {
    this.router.navigate(['home']);
  }

  public isSessionDisabled(session: QuizSession) {
    return (
      session.sessionState === QuizSessionStatus.Running ||
      session.countOfPlayers === session.maxPlayers
    );
  }

  private loadActiveSessions() {
    this.quizService
      .loadQuizSessions()
      .pipe(
        tap((quizSessions: QuizSession[]) => (this.quizSessions = quizSessions))
      )
      .subscribe();
  }
}
