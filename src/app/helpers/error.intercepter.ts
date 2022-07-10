import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Router } from '@angular/router';

import { AuthenticationService } from '../services/authenticationService';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private authenticationService: AuthenticationService,
        private router: Router
    ) { 
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError(err => {
                let handled: boolean = false;
                switch (err.status) {
                    case 401:      // UnAuthorized
                        console.log(`redirect to login`);
                        this.authenticationService.logout();
                        handled = true;
                        break;
                    case 403:     //forbidden
                        console.log(`redirect to login`);
                        this.authenticationService.logout();
                        handled = true;
                        break;
                }
                console.log(err);
                const error = err.error.message || err.statusText;
                return throwError(error);
            })
        )
    }

    // intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //     return next.handle(req).pipe(
    //       catchError((error) => {
    //         let handled: boolean = false;
    //         console.error(error);
    //         if (error instanceof HttpErrorResponse) {
    //           if (error.error instanceof ErrorEvent) {
    //             console.error("Error Event");
    //           } else {
    //             console.log(`error status : ${error.status} ${error.statusText}`);
    //             switch (error.status) {
    //               case 401:      //login
    //                 console.log(`redirect to login`);
    //                 this.authenticationService.logout();
    //                 handled = true;
    //                 break;
    //               case 403:     //forbidden
    //                 console.log(`redirect to login`);
    //                 this.authenticationService.logout();
    //                 handled = true;
    //                 break;
    //             }
    //           }
    //         }
    //         else {
    //           console.error("Other Errors");
    //         }
    //         if (handled) {
    //           console.log('return back ');
    //           return of(error);
    //         } else {
    //           console.log('throw error back to to the subscriber');
    //           return throwError(error);
    //         }
    //       })
    //     )
    //   }
}