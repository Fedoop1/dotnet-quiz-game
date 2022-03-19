import { QuestionContent } from './quiz-question-content.model';

export interface QuizRound {
  questionId: number;
  startAt?: Date;
  endAt?: Date;
  questionContent: QuestionContent;
}
