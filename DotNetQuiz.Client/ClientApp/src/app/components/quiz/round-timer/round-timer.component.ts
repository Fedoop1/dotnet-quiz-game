import { Component, Input } from '@angular/core';

@Component({
  selector: 'round-timer',
  templateUrl: 'round-timer.component.html',
  styleUrls: ['round-timer.component.scss'],
})
export class RoundTimerComponent {
  private timer: any;

  public minutesLeft: number = 0;
  public secondsLeft: number = 0;

  @Input() startAt: Date = new Date();
  @Input() endAt: Date = new Date();
  @Input() kind: 'spinner' | 'progressBar' = 'spinner';

  public get roundProgress(): number {
    const totalSeconds = (this.endAt.getTime() - this.startAt.getTime()) / 1000;

    let secondsPassed = (new Date().getTime() - this.startAt.getTime()) / 1000;

    let roundProgress = (secondsPassed / totalSeconds) * 100;

    return roundProgress > 100 ? 100 : Math.round(roundProgress);
  }

  public startTimer() {
    this.stopTimer();

    this.timer = setInterval(() => this.updateTimer(), 1000);
  }

  public stopTimer() {
    clearInterval(this.timer);
  }

  private updateTimer() {
    let leftInSeconds = (this.endAt.getTime() - Date.now()) / 1000;

    leftInSeconds = leftInSeconds < 0 ? 0 : leftInSeconds;

    this.secondsLeft = Math.floor(leftInSeconds % 60);
    this.minutesLeft = Math.floor(leftInSeconds / 60);
  }
}
