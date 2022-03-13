import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HomeComponent } from './components/home/home.component';
import { QuizService } from './services/quiz.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreateSessionComponent } from './components/create-session/create-session.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JoinSessionComponent } from './components/join-session/join-session.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CreateAccountComponent } from './components/create-account/create-account.component';
import { SessionLobbyComponent } from './components/session-lobby/session-lobby.component';
import { QuizConfigurationService } from './services/quiz-configuration.service';
import { QuizHostComponent } from './components/quiz/host/quiz-host.component';
import { QuizComponent } from './components/quiz/player/quiz.component';
import { RoundStatisticComponent } from './components/quiz/round-statistic/round-statistic.component';
import { LeaderBoardComponent } from './components/quiz/leader-board/leader-board.component';
import { SortPipe } from './components/quiz/leader-board/pipes/sort.pipe';
import { MatTabsModule } from '@angular/material/tabs';
import { QuestionComponent } from './components/quiz/question/question.component';
import { QuestionTypePipe } from './components/quiz/host/pipes/question-type.pipe';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RoundTimerComponent } from './components/quiz/round-timer/round-timer.component';
import { DateFormatterPipe } from './components/quiz/host/pipes/date-formatter.pipe';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CreateSessionComponent,
    JoinSessionComponent,
    CreateAccountComponent,
    SessionLobbyComponent,
    QuizHostComponent,
    QuizComponent,
    RoundStatisticComponent,
    LeaderBoardComponent,
    QuestionComponent,
    RoundTimerComponent,
    // Pipes
    SortPipe,
    QuestionTypePipe,
    DateFormatterPipe,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    HttpClientModule,
    RouterModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatButtonModule,
    MatTabsModule,
    MatProgressSpinnerModule,
  ],
  exports: [MatIconModule, MatButtonModule],
  providers: [QuizConfigurationService, QuizService],
  bootstrap: [AppComponent],
})
export class AppModule {}
