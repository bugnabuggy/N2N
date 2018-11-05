import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {
  PageNotFoundComponent,
  MainPageComponent,
  LoginPageComponent
} from './pages';

const routes: Routes = [
  {
    path: 'main',
    component: MainPageComponent
  },
  {
    path: 'login',
    component: LoginPageComponent
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
