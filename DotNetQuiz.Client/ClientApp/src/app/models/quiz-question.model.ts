import { QuestionType } from './enums/question-type.enum';
import { QuestionAnswer } from './quiz-question-answer.model';
import { QuestionContent } from './quiz-question-content.model';

export interface Question {
  questionId: number;
  questionReward: number;
  questionType: QuestionType;
  options: string[];
  content: QuestionContent;
  answer: QuestionAnswer;
}
