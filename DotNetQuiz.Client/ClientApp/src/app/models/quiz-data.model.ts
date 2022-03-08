import { QuizPlayer } from './quiz-player.model';

export interface QuizData {
  player: QuizPlayer;
  isHost: boolean;
  sessionId: string;
}
