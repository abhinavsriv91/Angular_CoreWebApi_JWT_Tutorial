import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { AuthGuard } from './helpers';
import { UserManagementComponent } from './user-management/user-management.component';


const routes: Routes = [
  { 
    path: 'login', 
    component: LoginComponent 
  },
  { 
    path: 'usermanagement', 
    component: UserManagementComponent, 
    canActivate:[AuthGuard], 
    data: {roles:['Admin']} 
  },
  { 
    path: '**', 
    component: LoginComponent
  }
  // otherwise redirect to home
  // { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
