import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil, tap } from 'rxjs/operators';
import { QuestionType } from 'src/app/models/enums/question-type.enum';
import { SessionState } from 'src/app/models/enums/round-state.enum.model';
import { QuizData } from 'src/app/models/quiz-data.model';
import { QuizPlayerAnswer } from 'src/app/models/quiz-player-answer.model';
import { Question } from 'src/app/models/quiz-question.model';
import { QuizRound } from 'src/app/models/quiz-round.model';
import { RoundStatistic } from 'src/app/models/round-statistic.model';
import { QuizService } from 'src/app/services/quiz.service';
import { DestroyableComponent } from 'src/app/utils/destroyable-component/destroyable.component';
import { byteArrayToBase64 } from 'src/app/utils/image.util';

@Component({
  selector: 'quiz',
  templateUrl: 'quiz.component.html',
  styleUrls: ['quiz.component.scss'],
})
export class QuizComponent extends DestroyableComponent implements OnInit {
  private readonly canvasDefaultSize = {
    width: 800,
    height: 400,
  };

  public quizData!: QuizData;
  public sessionState: SessionState = SessionState.Round;
  public quizRound!: QuizRound;

  public QuestionType = QuestionType;
  public SessionState = SessionState;

  @ViewChild('canvas') canvas!: ElementRef<HTMLCanvasElement>;

  public get canvasWidth(): number {
    return this.canvas.nativeElement.width;
  }

  public get canvasHeight(): number {
    return this.canvas.nativeElement.height;
  }

  public set canvasWidth(width: number) {
    this.canvas.nativeElement.height = width;
  }

  public set canvasHeight(height: number) {
    this.canvas.nativeElement.height = height;
  }

  constructor(
    private readonly quizService: QuizService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {
    super();
  }

  public submitAnswer(answer: string) {
    const questionAnswer: QuizPlayerAnswer = {
      playerId: this.quizData.player.id,
      answerContent: answer,
      answerTime: this.quizRound.startAt - Date.now(),
    };

    this.quizService
      .submitAnswer(questionAnswer, this.quizData.sessionId)
      .subscribe();
  }

  ngOnInit(): void {
    this.quizData = history.state.quiz;

    if (!this.quizData) this.router.navigate(['join-session']);

    this.setupSubscribers();
  }

  private setupSubscribers() {
    this.quizService.sessionState$
      .pipe(
        takeUntil(this.onDestroy$),
        tap((sessionState) => {
          this.sessionState = sessionState;
          this.processSessionStateChange();
        }),

        tap(() => this.cdr.markForCheck())
      )
      .subscribe();
  }

  private displayQuestion() {
    const context = this.canvas.nativeElement.getContext('2d');
    this.clearCanvas(context);

    if (this.quizRound.questionContent.questionText) {
      this.canvasWidth = this.canvasDefaultSize.width;
      this.canvasHeight = this.canvasDefaultSize.height;

      context?.fillText(
        this.quizRound.questionContent.questionText,
        this.canvasWidth / 2,
        this.canvasHeight / 2,
        this.canvasWidth / 3
      );
    } else if (this.quizRound.questionContent.questionBlob) {
      const image = new Image(this.canvasWidth, this.canvasHeight);
      image.src =
        'data:image/png;base64,' +
        byteArrayToBase64(this.quizRound.questionContent.questionBlob);

      context?.drawImage(image, 0, 0);
    }
  }

  private clearCanvas(context: CanvasRenderingContext2D | null) {
    context?.clearRect(0, 0, this.canvasWidth, this.canvasHeight);
  }

  private processSessionStateChange() {
    switch (this.sessionState) {
      case SessionState.Closed:
        this.router.navigate(['join-session'], {
          queryParams: { sessionClosed: true },
        });
        return;
      case SessionState.Round:
        this.quizService
          .getQuizRound(this.quizData.sessionId)
          .pipe(
            takeUntil(this.onDestroy$),
            tap((quizRound) => {
              this.quizRound = quizRound;
              this.displayQuestion();
            })
          )
          .subscribe();
        return;
      case SessionState.RoundStatistic:
    }
  }
}
