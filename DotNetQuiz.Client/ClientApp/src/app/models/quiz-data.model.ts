import { QuizPlayer } from './quiz-player.model';
import { Question } from './quiz-question.model';

export interface QuizData {
  player: QuizPlayer;
  isHost: boolean;
  sessionId: string;
}
