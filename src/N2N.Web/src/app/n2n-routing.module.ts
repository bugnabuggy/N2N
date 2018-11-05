import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {
  PageNotFoundComponent,
  MainPageComponent,
  LoginPageComponent,
  RegistrationPageComponent,
  ResetPasswordPageComponent
} from './pages';
import { Endpoints } from './enums/endpoints';

const routes: Routes = [
  {
    path: 'main',
    component: MainPageComponent
  },
  {
    path: Endpoints.site.login,
    component: LoginPageComponent
  },
  {
    path: Endpoints.site.registration,
    component: RegistrationPageComponent
  },
  {
    path: Endpoints.site.resetPassword,
    component: ResetPasswordPageComponent
  },
  {
    path: '',
    redirectTo: '/main',
    pathMatch: 'full'
  },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class N2NRoutingModule { }
