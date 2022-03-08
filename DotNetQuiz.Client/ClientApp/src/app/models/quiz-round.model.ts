import { QuestionContent } from './quiz-question-content.model';

export interface QuizRound {
  startAt: number;
  endAt: number;
  questionContent: QuestionContent;
}
