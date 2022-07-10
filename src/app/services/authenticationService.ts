import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import jwt_decode, { JwtPayload }  from 'jwt-decode';

import { User, AuthenticatedUser } from '../models';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    
    private currentUserSubject: BehaviorSubject<AuthenticatedUser>;
    public currentUser: Observable<AuthenticatedUser>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<AuthenticatedUser>(JSON.parse(localStorage.getItem('currentUser') || '{}'));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): AuthenticatedUser {
        return this.currentUserSubject.value;
    }

    login(user: User) {
        let url:string = `${environment.baseUrl + environment.authenticationApi}user/authenticate`
        console.log('url-', url)
        return this.http.post<any>(url, user)
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }

    getToken(): string {
        return this.currentUserValue.token!;
    }
    
    getTokenExpirationDate(token: string): Date {
        const decoded = jwt_decode<JwtPayload>(token)
        
        if (decoded.exp === undefined) return null as any;
        const date = new Date(0); 
        
        date.setUTCSeconds(decoded.exp);
        return date;
    }

    isTokenExpired(token?: string): boolean {
        if(!token) token = this.getToken();
        if(!token) return true;
        
        const date = this.getTokenExpirationDate(token);
        if(date === undefined) return false;

        return !(date.valueOf() > new Date().valueOf());
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null as any);
    }
}