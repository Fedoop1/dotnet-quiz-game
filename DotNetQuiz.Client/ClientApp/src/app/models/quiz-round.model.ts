import { QuestionContent } from './quiz-question-content.model';
import { Question } from './quiz-question.model';

export interface QuizRound {
  question: Question;
  startAt?: Date;
  endAt?: Date;
}
