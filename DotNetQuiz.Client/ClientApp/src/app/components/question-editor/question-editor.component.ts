import { Component, Input, OnInit } from '@angular/core';
import { QuestionContentType } from 'src/app/models/enums/question-content-type.enum';
import { QuestionType } from 'src/app/models/enums/question-type.enum';
import { InfoModel } from 'src/app/models/info-model.model';
import { Question } from 'src/app/models/quiz-question.model';

@Component({
  selector: 'question-editor',
  templateUrl: 'question-editor.component.html',
  styleUrls: ['question-editor.component.scss'],
})
export class QuestionEditorComponent implements OnInit {
  @Input() question!: Question;

  public questionContentType!: QuestionContentType;

  public questionsTypeInfoModels = Object.entries(QuestionType)
    .filter(([_, value]) => typeof value === 'number')
    .map(([key, value]) => {
      return { name: key, value } as InfoModel;
    });

  QuestionType = QuestionType;
  QuestionContentType = QuestionContentType;

  constructor() {}

  ngOnInit() {
    this.questionContentType = this.question.content.questionText
      ? QuestionContentType.Text
      : this.question.content.questionBlob
      ? QuestionContentType.Blob
      : QuestionContentType.Text;
  }

  public questionContentTypeChange() {
    this.question.content = {};
  }

  public questionTypeChange() {
    this.question.options = [];
  }

  public async questionContentFileChange($event: any) {
    if (!$event || !$event.files || !$event.files.length) return;

    this.question.content.questionBlob = [];

    const file = $event.files[0] as File;
    const fileContent = await file.arrayBuffer();
    const fileContentInUint8 = new Uint8Array(fileContent);

    // TODO: Verify this logic
    for (let index = 0; index < fileContentInUint8.length; index++) {
      this.question.content.questionBlob[index] = fileContentInUint8[index];
    }
  }

  public removeOptionClick(index: number) {
    this.question.options.splice(index, 1);
  }

  public saveOptionClick(index: number, value: string) {
    this.question.options[index] = value;
  }

  public addQuestionOptionClick() {
    this.question.options = this.question.options || [];

    this.question.options.push('');
  }
}
