import { QuizSessionStatus } from './quiz-session-status.enum';

export interface QuizSession {
  countOfPlayers: number;
  maxPlayers: number;
  sessionId: string;
  sessionState: QuizSessionStatus;
}
