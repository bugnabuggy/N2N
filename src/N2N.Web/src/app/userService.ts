import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class UserService {
  private _jsonHeaders = new Headers({
    'Content-Type': 'application/json charset=utf-8',
  });
  private UserControllerUrl = 'http://localhost:63354/User/register';
  // URL to web api

  constructor(private http: Http) { }
 
  sendUserDataForRegistration(nickName:string, password:string, capcha:string): Promise<any> {
    
    var data = {  
                nickName, 
                password,
                capcha
               };
      return this.http.post(this.UserControllerUrl, data, { headers: this._jsonHeaders })
            .toPromise()
            .then(resp => { return resp })
            .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {

    return Promise.reject(error);;
  }

}
