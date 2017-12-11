import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import {StoreHeaders} from './storeHeaders';
import {StoreLinks} from './storeLinks';


@Injectable()
export class UserService {
  

  
  // URL to web api

  constructor(
    private http: Http,
    private _storeHeaders:StoreHeaders,
    private _storeLinks:StoreLinks
  ) { }

  sendUserDataForRegistration(nickName: string, password: string, capcha: string): Promise<any> {

    var data = {
      nickName,
      password,
      capcha
    };
    return this.http.post(
        this._storeLinks.registerUrl,
        data,
        { headers: this._storeHeaders.jsonHeader }
      )
      .toPromise()
      .then(resp => { debugger; return resp })
      .catch(this.handleError);
  }

  checkUser(){
    
    return this.http.get(
      this._storeLinks.СheckUserUrl,
      { headers: this._storeHeaders.jsonAndTokenHeaders }
    )
    .toPromise()
    .then(resp => { debugger; return resp.json })
    .catch(this.handleError);
  }

  logOut(){
    return this.http.delete(
      this._storeLinks.logoutUrl,
      { headers:  this._storeHeaders.jsonAndTokenHeaders}
    )
    .toPromise()
  }

  private handleError(error: any): Promise<any> {

    return Promise.reject(error);;
  }

}
