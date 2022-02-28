import { Component } from '@angular/core';
import { QuizService } from 'src/app/services/quiz.service';

@Component({
  selector: 'create-account',
  templateUrl: 'create-account.component.html',
  styleUrls: ['create-account.component.scss'],
})
export class CreateAccountComponent {
  public userName!: string;
  constructor(private readonly quizService: QuizService) {}
}
