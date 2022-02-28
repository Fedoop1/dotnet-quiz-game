import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from 'src/app/services/quiz.service';

@Component({
  selector: 'create-account',
  templateUrl: 'create-account.component.html',
  styleUrls: ['create-account.component.scss'],
})
export class CreateAccountComponent {
  private readonly maxIndexValue = 2147483647;
  private readonly minIndexValue = 0;

  private userId!: number;
  public nickName!: string;
  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {
    this.userId = this.generateUserId();
  }

  public onJoinButtonClick() {
    this.quizService.addPlayer(
      this.nickName,
      this.userId,
      this.route.snapshot.queryParams?.sessionId
    );
  }

  public onBackButtonClick() {
    this.router.navigate(['join-session']);
  }

  private generateUserId() {
    return Math.floor(Math.random() * this.maxIndexValue - this.minIndexValue);
  }
}
