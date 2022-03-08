import { Pipe, PipeTransform } from '@angular/core';
import { QuestionType } from 'src/app/models/enums/question-type.enum';
import QuestionTypeStrings from '../models/question-type-strings.constants';

@Pipe({ name: 'questionType' })
export class QuestionTypePipe implements PipeTransform {
  transform(value: QuestionType) {
    return Object(QuestionTypeStrings)[value];
  }
}
