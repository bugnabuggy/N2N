import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';

import { Endpoints } from '../enums/endpoints';
import { of, Observable } from 'rxjs';
import { NotificationService } from './notification.service';
import { SiteConstants } from '../enums/site-constants';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient,
    private notifications: NotificationService,
  ) { }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // // TODO: better job of transforming error for user consumption
      // this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }


  register(nickname: string, password: string) {

    return this.http.post(
      Endpoints.api.register,
      {
        nickname,
        password,
      }).pipe(
        tap(_ => { console.log(_); }),
        // catchError(this.handleError('login', []))
      );
  }

  login(username: string, password: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded',
      })
    };
    const body = new HttpParams()
      .set('client_id', SiteConstants.clientId)
      .set('client_secret', SiteConstants.clientSecred)
      .set('grant_type', 'password')
      .set('username', username)
      .set('password', password);

    return this.http.post(
      Endpoints.api.identityServerLogin,
      body
    ).pipe(
      tap(_ => { console.log(_); }),
      // catchError(this.handleError('login', []))
    );
  }
}
