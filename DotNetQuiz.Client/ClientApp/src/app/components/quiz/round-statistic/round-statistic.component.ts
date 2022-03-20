import { Component, destroyPlatform, Input, OnInit } from '@angular/core';
import { takeUntil, tap } from 'rxjs/operators';
import { RoundStatistic } from 'src/app/models/round-statistic.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';

@Component({
  selector: 'round-statistic',
  templateUrl: 'round-statistic.component.html',
  styleUrls: ['round-statistic.component.scss'],
})
export class RoundStatisticComponent
  extends DestroyableComponent
  implements OnInit
{
  public roundStatistic!: RoundStatistic;

  @Input() sessionId!: string;

  //TODO: Add round statistic displaying
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
      .getRoundStatistic(this.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        tap((roundStatistic) => (this.roundStatistic = roundStatistic))
      )
      .subscribe();
  }
}
