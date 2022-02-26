import { QuestionAnswer } from './question-answer.model';
import { QuestionContent } from './question-content.model';

export interface Question {
  questionId: number;
  questionReward: number;
  content: QuestionContent;
  answer: QuestionAnswer;
}

/*
"questionId": 1,
        "questionReward": 100,
        "content": {
          "questionText": "text"
        },
        "answer": {
          "answerContent": "answer"
        }
*/
