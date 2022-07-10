import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class ManagementService {
    
    private baseUrl = `${environment.baseUrl + environment.managementApi}`;

    constructor(private http: HttpClient) {
        
    }

    /**
    * get call to Management SPOT API
    */
    get(url: string): Observable<any> {
        return this.http.get(this.baseUrl + url)
        .pipe(
            catchError((err) => {
              return throwError(err);
            })
        );
        // .pipe(
        //     catchError(error => {
        //         let errorMsg: string;
        //         if (error.error instanceof ErrorEvent) {
        //             errorMsg = `Error: ${error.error.message}`;
        //         } else {
        //             errorMsg = this.getServerErrorMessage(error);
        //         }
        //         return throwError(errorMsg);
        //     })
        // );
    }
    
    /**
    * Post call to Management SPOT API
    */
    post(url:string, requestBody: any): Observable<any> {
        return this.http.post(`${this.baseUrl}${url}`, requestBody)
        .pipe(
            catchError((err) => {
              return throwError(err);
            })
        );
        // .pipe(
        //     catchError(error => {
        //         let errorMsg: string;
        //         if (error.error instanceof ErrorEvent) {
        //             errorMsg = `Error: ${error.error.message}`;
        //         } else {
        //             errorMsg = this.getServerErrorMessage(error);
        //         }
        //         return throwError(errorMsg);
        //     })
        // );
    }
    
    /**
    * put call to Management SPOT API
    */
    put(url:string, requestBody:any): Observable<any> {
        return this.http.put(`${this.baseUrl}${url}`, requestBody)
        .pipe(
            catchError((err) => {
                return throwError(err);
            })
        );
        // );
        // .pipe(
        //     catchError(error => {
        //         let errorMsg: string;
        //         if (error.error instanceof ErrorEvent) {
        //             errorMsg = `Error: ${error.error.message}`;
        //         } else {
        //             errorMsg = this.getServerErrorMessage(error);
        //         }
        //         return throwError(errorMsg);
        //     })
        // );
    }
    
    /**
    * delete call to Management SPOT API
    */
    delete(url: string): Observable<any> {
        return this.http.delete(`${this.baseUrl}${url}`)
        .pipe(
            catchError((err) => {
              return throwError(err);
            })
        );
        // .pipe(
        //     catchError(error => {
        //         console.log('error-', error)
        //         let errorMsg: string;
        //         if (error.error instanceof ErrorEvent) {
        //             console.log('1-', error.error.message)
        //             errorMsg = `Error: ${error.error.message}`;
        //         } else {
        //             console.log('2-', this.getServerErrorMessage(error))
        //             errorMsg = this.getServerErrorMessage(error);
        //         }

        //         return throwError(errorMsg);
        //     })
        // );
    }

    /**
    * Extract and prepare server error message
    */
    private getServerErrorMessage(error: HttpErrorResponse): string {
        switch (error.status) {
            case 401: {
                return `UnAuthenticated: ${error.message}`;
            }
            case 403: {
                return `Access Denied: ${error.message}`;
            }
            case 404: {
                return `Not Found: ${error.message}`;
            }
            case 405: {
                return `Method not allowed: ${error.message}`;
            }
            case 422: {
                return `Error: ${error.message}`;
            }
            case 500: {
                return `Internal Server Error: ${error.message}`;
            }
            default: {
                return `Unknown Server Error: ${error.message}`;
            }
        }
    }
}