import { Component, Input } from '@angular/core';
import { Question } from 'src/app/models/quiz-question.model';

@Component({
  selector: 'question-list',
  templateUrl: 'question-list.component.html',
  styleUrls: ['question-list.component.scss'],
})
export class QuestionListComponent {
  @Input() questions: Question[] = [];
  @Input() selectedQuestionId?: number;

  public isSelectedQuestion(questionId: number) {
    return questionId === this.selectedQuestionId;
  }
}
