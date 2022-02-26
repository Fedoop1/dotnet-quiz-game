import { QuestionPack } from './questio-pack.model';

export interface QuizConfiguration {
  roundDuration: number;
  maxPlayers: number;
  streakMultiplier: number;
  timeMultiplier: number;
  answerIgnoreCase: boolean;
  questionPack: QuestionPack;
}

/*
{
  // Seconds
  "roundDuration": 60,
  "maxPlayers": 4,
  "streakMultiplier": 1,
  "timeMultiplier": 1,
  "answerIgnoreCase": false,
  "questionPack": {
    "questionPackId": 1,
    "questions": [
      {
        "questionId": 1,
        "questionReward": 100,
        "content": {
          "questionText": "text"
        },
        "answer": {
          "answerContent": "answer"
        }
      }
    ]
  }
}

*/
