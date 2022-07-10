import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../services/authenticationService';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  loggedInUserName: string = ''

  constructor(
    private authService: AuthenticationService,
    private router: Router
  ){
    if (this.authService.currentUserValue) { 
      this.loggedInUserName = this.authService.currentUserValue.fullName
    }
  }

  ngOnInit(): void {
  }

  logout() {
    console.log("logout")
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
