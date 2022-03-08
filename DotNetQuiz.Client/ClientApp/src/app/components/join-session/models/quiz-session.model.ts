import { SessionState } from 'src/app/models/enums/round-state.enum.model';

export interface QuizSession {
  countOfPlayers: number;
  maxPlayers: number;
  sessionId: string;
  sessionState: SessionState;
}
