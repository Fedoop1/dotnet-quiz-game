import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Color } from 'chart.js';
import * as _ from 'lodash';
import { finalize, takeUntil, tap } from 'rxjs/operators';
import { RoundStatistic } from 'src/app/models/round-statistic.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';
import { ChartData } from './models/chart-data.model';

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
  public chartData: ChartData[] = [];

  @Input() sessionId!: string;
  @Input() size!: 'sm' | 'md' | 'lg';

  constructor(private readonly quizService: QuizService) {
    super();
  }

  ngOnInit(): void {
    this.loadData();
  }

  public updateData() {
    this.loadData();
  }

  public loadData() {
    this.quizService
      .getRoundStatistic(this.sessionId)
      .pipe(
        takeUntil(this.onDestroy$),
        tap((roundStatistic) => {
          this.roundStatistic = roundStatistic;
          this.chartData =
            roundStatistic.answerStatistic &&
            roundStatistic.answerStatistic.length
              ? roundStatistic.answerStatistic.map(({ key, value }) => {
                  return {
                    name: key,
                    value: value,
                  };
                })
              : [];
        })
      )
      .subscribe();
  }
}
