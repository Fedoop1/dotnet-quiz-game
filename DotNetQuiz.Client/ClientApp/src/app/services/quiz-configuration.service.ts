import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { QuestionPack } from '../models/quiz-question-pack.model';
import { QuizConfiguration } from '../models/quiz-configuration.model';
import * as _ from 'lodash';
import { Question } from '../models/quiz-question.model';
import { QuestionType } from '../models/enums/question-type.enum';

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

  public validateQuestionPack(questionPack?: QuestionPack): boolean {
    return (questionPack?.questions &&
      !!questionPack?.questions.length &&
      questionPack.questions.every((question) =>
        this.validateQuestion(question)
      ))!;
  }

  // TODO: Refactor this hard coded validation
  private validateQuestion(question: Question): boolean {
    if (!question) return false;

    if (question.questionReward < 0) return false;
    if (question.questionType === undefined) return false;
    if (
      question.questionType === QuestionType.Select &&
      (!question.options || !question.options.length)
    )
      return false;

    if (!question.content.questionBlob && !question.content.questionText)
      return false;
    if (!question.answer.answerContent) return false;

    return true;
  }
}
