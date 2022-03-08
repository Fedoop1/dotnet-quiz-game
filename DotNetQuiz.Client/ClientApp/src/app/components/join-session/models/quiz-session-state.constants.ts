import { SessionState } from 'src/app/models/enums/round-state.enum.model';

export default class QuizSessionStateStrings {
  public static readonly [SessionState.NotStarted] = 'Not started';
  public static readonly [SessionState.LeaderBoard] = 'Leader board';
  public static readonly [SessionState.Round] = 'Round';
  public static readonly [SessionState.Closed] = 'Closed';
  public static readonly [SessionState.RoundStatistic] = 'Round statistic';
}
