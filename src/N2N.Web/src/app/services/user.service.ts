import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';

import { Endpoints } from '../enums/endpoints';
import { of, Observable } from 'rxjs';
import { NotificationService } from './notification.service';

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
      debugger;
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
        password
      }).pipe(
        tap(_ => { console.log(_); }),
        // catchError(this.handleError('login', []))
      );
  }

  login(username: string, password: string) {
    return this.http.post(
      Endpoints.api.login,
      {
        username,
        password
      }).pipe(
        tap(_ => { console.log(_); }),
        // catchError(this.handleError('login', []))
      );
  }
}
