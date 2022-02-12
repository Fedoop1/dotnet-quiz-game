﻿using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuizSessionHandler
    {
        Guid QuizHandlerId { get; }
        QuizRound CurrentSessionRound { get; }
        IReadOnlyCollection<QuizPlayer> SessionPlayers { get; }
        SessionState SessionState { get; }
        void UploadQuizConfiguration(QuizConfiguration configuration);
        void AddPlayerToSession(QuizPlayer quizPlayer);
        void RemovePlayerFromSession(int playerId);
        void StartGame();
        void SubmitAnswer(QuizPlayerAnswer answer);
        void NextRound();
        RoundStatistic BuildCurrentRoundStatistic();
    }
}