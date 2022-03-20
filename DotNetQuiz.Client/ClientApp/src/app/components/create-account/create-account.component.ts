import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { QuizService } from 'src/app/services/quiz.service';

@Component({
  selector: 'create-account',
  templateUrl: 'create-account.component.html',
  styleUrls: ['create-account.component.scss'],
})
export class CreateAccountComponent {
  public nickName!: string;

  public get isInvalidNickname(): boolean {
    return !this.nickName || !this.nickName?.trim().length;
  }

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {}

  public onJoinButtonClick() {
    if (this.isInvalidNickname) {
      return;
    }

    this.router.navigate(['session-lobby'], {
      queryParams: {
        sessionId: this.route.snapshot.queryParams?.sessionId,
        nickName: this.nickName,
      },
    });
  }

  public onBackButtonClick() {
    this.router.navigate(['join-session']);
  }
}
