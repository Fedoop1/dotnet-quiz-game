import { QuestionContent } from './quiz-question-content.model';

export interface QuizRound {
  startAt?: Date;
  endAt?: Date;
  questionContent: QuestionContent;
}
