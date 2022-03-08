import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { QuestionPack } from '../models/quiz-question-pack.model';
import { QuizConfiguration } from '../models/quiz-configuration.model';

@Injectable()
export class QuizConfigurationService {
  public parseQuestionPack(file: File): Observable<QuestionPack | undefined> {
    return from(file.text()).pipe(
      map((fileContent) => {
        const questionPack = JSON.parse(fileContent);
        return this.validateQuestionPack(questionPack)
          ? questionPack
          : undefined;
      })
    );
  }

  public validateQuizConfiguration(
    quizConfiguration: QuizConfiguration
  ): boolean {
    return (
      quizConfiguration.roundDuration >= 5 &&
      quizConfiguration.maxPlayers > 0 &&
      quizConfiguration.streakMultiplier >= 1 &&
      quizConfiguration.timeMultiplier >= 1 &&
      this.validateQuestionPack(quizConfiguration.questionPack!)
    );
  }

  private validateQuestionPack(questionPack?: QuestionPack): boolean {
    return (
      (questionPack?.questions &&
        questionPack.questions.every(
          (question) =>
            question.questionId &&
            question.questionReward >= 0 &&
            question.content &&
            question.answer
        )) ||
      false
    );
  }
}
