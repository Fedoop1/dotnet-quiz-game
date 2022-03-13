import { Component, Input } from '@angular/core';

@Component({
  selector: 'round-timer',
  templateUrl: 'round-timer.component.html',
  styleUrls: ['round-timer.component.scss'],
})
export class RoundTimerComponent {
  private timer: any;

  private minutesLeft: number = 0;
  private secondsLeft: number = 0;

  @Input() startAt: Date = new Date();
  @Input() endAt: Date = new Date();

  public get roundProgress(): number {
    const roundProgress =
      ((this.endAt.getTime() - this.startAt.getTime() / 1000) * 100) / 60;
    return roundProgress > 100 ? 100 : roundProgress;
  }

  public get timesLeft(): string {
    return `${this.minutesLeft}:${this.secondsLeft}`;
  }

  public startTimer() {
    this.stopTimer();

    this.timer = setInterval(() => this.updateTimer(), 1000);
  }

  public stopTimer() {
    clearInterval(this.timer);
  }

  private updateTimer() {
    let leftInMilliseconds = this.endAt.getTime() - Date.now();

    leftInMilliseconds = leftInMilliseconds < 0 ? 0 : leftInMilliseconds;

    this.secondsLeft = (leftInMilliseconds / 1000) % 60;
    this.minutesLeft = leftInMilliseconds / 1000 / 60;
  }
}
