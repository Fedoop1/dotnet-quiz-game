import { Question } from './question.model';

export interface QuestionPack {
  questionPackId: number;
  questions: Question[];
}
