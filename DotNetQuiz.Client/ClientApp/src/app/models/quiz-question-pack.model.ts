import { Question } from './quiz-question.model';

export interface QuestionPack {
  questionPackId: number;
  questions: Question[];
}
