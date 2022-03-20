import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { QuestionType } from 'src/app/models/enums/question-type.enum';
import { Question } from 'src/app/models/quiz-question.model';
import { QuizRound } from 'src/app/models/quiz-round.model';
import { byteArrayToBase64 } from 'src/app/utils/image.util';

@Component({
  selector: 'question',
  templateUrl: 'question.component.html',
  styleUrls: ['question.component.scss'],
})
export class QuestionComponent {
  private readonly canvasDefaultSize = {
    width: 800,
    height: 400,
  };

  public QuestionType = QuestionType;

  @ViewChild('canvas') canvas!: ElementRef<HTMLCanvasElement>;

  @Input() quizRound!: QuizRound;
  @Input() submitAnswer!: (answer: string) => void;
  @Input() isHostView: boolean = false;
  @Input() isButtonDisabled: boolean = false;

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

  public displayQuestion() {
    const context = this.canvas.nativeElement.getContext('2d')!;
    this.clearCanvas(context);

    if (this.quizRound.question.content.questionText) {
      this.canvasWidth = this.canvasDefaultSize.width;
      this.canvasHeight = this.canvasDefaultSize.height;

      const thirdOfWidth = this.canvasWidth / 3;
      const thirdOfHeight = this.canvasHeight / 3;
      const fontHeight = 43;
      const wordsDistance = fontHeight;

      this.setupFont(context, fontHeight);

      if (
        context.measureText(this.quizRound.question.content.questionText)
          .width > thirdOfWidth
      ) {
        const splittedText = this.splitText(
          this.quizRound.question.content.questionText,
          context,
          thirdOfWidth + thirdOfWidth * 0.5
        );

        const totalHeight = fontHeight * splittedText.length;

        if (totalHeight > this.canvasHeight - thirdOfHeight * 2) {
          this.canvasHeight = totalHeight + 250;
          this.setupFont(context, fontHeight);
        }

        for (let index = 0; index < splittedText.length; index++) {
          const textPart = splittedText[index];

          context.fillText(
            textPart,
            thirdOfWidth + thirdOfWidth * 0.5,
            thirdOfHeight + fontHeight + index * wordsDistance,
            thirdOfWidth + thirdOfWidth * 0.5
          );
        }

        return;
      }

      context.fillText(
        this.quizRound.question.content.questionText,
        thirdOfWidth,
        thirdOfHeight + fontHeight,
        thirdOfWidth
      );
    } else if (this.quizRound.question.content.questionBlob) {
      const image = new Image(this.canvasWidth, this.canvasHeight);
      image.src =
        'data:image/png;base64,' +
        byteArrayToBase64(this.quizRound.question.content.questionBlob);

      context.drawImage(image, 0, 0);
    }
  }

  private clearCanvas(context: CanvasRenderingContext2D) {
    context.clearRect(0, 0, this.canvasWidth, this.canvasHeight);
  }

  private splitText(
    text: string,
    context: CanvasRenderingContext2D,
    maxWidth: number
  ): string[] {
    const result: string[] = [];
    const words = text.split(' ');
    let line: string[] = [];

    words.forEach((word) => {
      line.push(word);

      if (context.measureText(line.join(' ')).width > maxWidth) {
        line.pop();
        result.push(line.join(' '));
        line = [word];
      }
    });

    return [...result, ...line];
  }

  private setupFont(context: CanvasRenderingContext2D, fontHeight: number) {
    context.font = `bold ${fontHeight}px Helvetica, Arial, sans-serif`;
    context.fillStyle = '#c9c5c0';
    context.textAlign = 'center';
  }
}
