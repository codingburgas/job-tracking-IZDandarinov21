import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { HomeComponent } from './components/pages/home/home.component';
import { DashboardComponent } from './components/pages/dashboard/dashboard.component';
import { JobAdvertisementListComponent } from './components/job-advertisement/job-advertisement-list/job-advertisement-list.component'; 
import { JobAdvertisementFormComponent } from './components/job-advertisement/job-advertisement-form/job-advertisement-form.component'; 
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  // Маршрути за обяви за работа (достъпни само за логнати потребители)
  {
    path: 'job-advertisements',
    component: JobAdvertisementListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'job-advertisements/add',
    component: JobAdvertisementFormComponent,
    canActivate: [authGuard]
  },
  {
    path: 'job-advertisements/edit/:id', // Маршрут за редактиране с параметър ID
    component: JobAdvertisementFormComponent,
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: '/home' }
];
