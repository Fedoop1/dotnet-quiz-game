import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
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
  public quizSessions: QuizSession[] = [];

  public QuizSessionStatus = QuizSessionStatus;

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    if (this.route.snapshot.queryParams?.sessionClosed === 'true')
      alert('Session closed. Host left the session');

    this.loadActiveSessions();
  }

  public onRefreshButtonClick() {
    this.loadActiveSessions();
  }

  public onJoinButtonClick(sessionId: string) {
    this.router.navigate(['create-account'], {
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
      .getQuizSessions()
      .pipe(
        tap((quizSessions: QuizSession[]) => (this.quizSessions = quizSessions))
      )
      .subscribe();
  }
}
