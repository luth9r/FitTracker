import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/components/login/login.component';
import { HomeComponent } from './features/home/components/home.component';
import { LoginGuard } from './features/auth/guards/login.guard';
import { AuthGuard } from './features/auth/guards/auth.guard';
import { ExercisesComponent } from './features/exercises/components/exercises/exercises.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    //canActivate: [LoginGuard]
  },
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
    //canActivate: [AuthGuard]
  },
  {
    path: 'exercises',
    component: ExercisesComponent
    //canActivate: [AuthGuard]
  }
];
