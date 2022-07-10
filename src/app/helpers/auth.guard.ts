import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from '../services/authenticationService';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    private allowedRoles: string[] = [];

    constructor(
        private router: Router,
        private authService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authService.currentUserValue;
        
        if (currentUser && !this.authService.isTokenExpired()) {
            this.allowedRoles = route.data["roles"];
            if(this.allowedRoles){
                const allowed : boolean = currentUser.roles.filter( 
                role => this.allowedRoles.includes(role)).length > 0;
                if(allowed){
                    // if role is allowed, return true
                    return true;
                }
            }
            else{
                // if allowed role is not available and 
                // user is logged and token not expired, return true
                return true;
            }
        }

        // not passed above three criteria so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}