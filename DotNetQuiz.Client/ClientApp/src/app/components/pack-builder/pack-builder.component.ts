import {
  Component,
  Inject,
  Input,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as _ from 'lodash';
import { QuestionType } from 'src/app/models/enums/question-type.enum';
import { QuestionPack } from 'src/app/models/quiz-question-pack.model';
import { Question } from 'src/app/models/quiz-question.model';
import { QuizConfigurationService } from 'src/app/services/quiz-configuration.service';

@Component({
  selector: 'pack-builder',
  templateUrl: 'pack-builder.component.html',
  styleUrls: ['pack-builder.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class PackBuilderComponent implements OnInit {
  public readonly questionPack: QuestionPack;
  public showErrorMessage: boolean = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) questionPack: QuestionPack | undefined,
    private readonly dialogRef: MatDialogRef<
      PackBuilderComponent,
      QuestionPack | undefined
    >,
    private readonly quizConfigurationService: QuizConfigurationService
  ) {
    this.questionPack = _.cloneDeep(questionPack) || ({} as QuestionPack);
  }

  ngOnInit() {}

  public addQuestionClick() {
    this.questionPack.questions = this.questionPack.questions || [];

    this.questionPack.questions.push({
      questionId: this.questionPack.questions.length + 1,
      questionReward: 100,
      questionType: QuestionType.Type,
      content: {},
      answer: {},
    } as Question);
  }

  public removeQuestionClick(questionId: number) {
    _.remove(
      this.questionPack.questions,
      (question) => question.questionId === questionId
    );
  }

  public onSaveClick() {
    if (!this.quizConfigurationService.validateQuestionPack(this.questionPack))
      return (this.showErrorMessage = true);

    return this.dialogRef.close(this.questionPack);
  }

  public onCloseClick() {
    this.dialogRef.close();
  }
}
