import { QuestionType } from './enums/question-type.enum';

export interface QuestionContent {
  questionType: QuestionType;
  questionText?: string;
  questionBlob?: number[];
  questionOptions?: string[];
}
