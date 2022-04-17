import { Component, Input, OnInit } from '@angular/core';
import { takeUntil, tap } from 'rxjs/operators';
import { QuizPlayer } from 'src/app/models/quiz-player.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';

@Component({
  selector: 'leader-board',
  templateUrl: 'leader-board.component.html',
  styleUrls: ['leader-board.component.scss'],
})
export class LeaderBoardComponent
  extends DestroyableComponent
  implements OnInit
{
  public quizPlayers: QuizPlayer[] = [];

  public orderByScore = (lhs: QuizPlayer, rhs: QuizPlayer) => {
    if (!lhs.score && !rhs.score) {
      return 0;
    }

    if (!lhs.score) {
      return 1;
    }

    if (!rhs.score) {
      return 1;
    }

    return lhs.score - rhs.score;
  };

  @Input() sessionId!: string;

  constructor(private readonly quizService: QuizService) {
    super();
  }

  ngOnInit(): void {
    this.loadData();
  }

  public updateData() {
    this.loadData();
  }

  private loadData() {
    this.quizService
      .getSessionPlayers(this.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        tap(
          (quizPlayers) => (this.quizPlayers = this.quizPlayers = quizPlayers)
        )
      )
      .subscribe();
  }
}
