import { Pipe, PipeTransform } from '@angular/core';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import QuizSessionStateStrings from '../models/quiz-session-state.constants';

@Pipe({ name: 'sessionState' })
export class SessionStatePipe implements PipeTransform {
  transform(value: SessionState) {
    return Object(QuizSessionStateStrings)[value];
  }
}
