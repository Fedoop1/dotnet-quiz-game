import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateSessionComponent } from './components/create-session/create-session.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'create-session', component: CreateSessionComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', component: HomeComponent },
];

@NgModule({ imports: [RouterModule.forRoot(routes)], exports: [RouterModule] })
export class AppRoutingModule {}
