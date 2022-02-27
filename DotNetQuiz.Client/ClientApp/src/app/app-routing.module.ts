import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateSessionComponent } from './components/create-session/create-session.component';
import { HomeComponent } from './components/home/home.component';
import { JoinSessionComponent } from './components/join-session/join-session.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'create-session', component: CreateSessionComponent },
  { path: 'join-session', component: JoinSessionComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', component: HomeComponent },
];

@NgModule({ imports: [RouterModule.forRoot(routes)], exports: [RouterModule] })
export class AppRoutingModule {}
