import { Component, Input } from '@angular/core';
import { QuestionType } from 'src/app/models/enums/question-type.enum';
import { Question } from 'src/app/models/quiz-question.model';
import { QuizRound } from 'src/app/models/quiz-round.model';

@Component({
  selector: 'question',
  templateUrl: 'question.component.html',
  styleUrls: ['question.component.scss'],
})
export class QuestionComponent {
  public QuestionType = QuestionType;

  @Input() quizRound!: QuizRound;
  @Input() submitAnswer!: (answer: string) => void;
  @Input() isHostView: boolean = false;
}
