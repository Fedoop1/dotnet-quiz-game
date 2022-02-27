import { Pipe, PipeTransform } from '@angular/core';
import QuizSessionStatusStrings from '../models/quiz-session-status.constants';
import { QuizSessionStatus } from '../models/quiz-session-status.enum';

@Pipe({ name: 'sessionStatus' })
export class SessionStatusPipe implements PipeTransform {
  transform(value: QuizSessionStatus) {
    return Object(QuizSessionStatusStrings)[value];
  }
}
