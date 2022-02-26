import { QuestionPack } from '../questio-pack.model';

export const DefaultQuestionPack: QuestionPack = {
  questionPackId: 1,
  questions: [
    {
      questionId: 1,
      questionReward: 100,
      content: { questionText: 'question' },
      answer: { answerContent: 'answer' },
    },
  ],
};
