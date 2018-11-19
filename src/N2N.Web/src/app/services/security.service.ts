import { Injectable } from '@angular/core';
import { LoginContract } from '../models';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  private _accessToken = '';
  private _refreshToken = '';
  private _expiration: Date;


  get accessToken() { return this._accessToken; }
  get refreshToken() { return this._refreshToken; }

  constructor() {
    debugger;
    this._accessToken = localStorage.getItem('access_token');
    this._refreshToken = localStorage.getItem('refresh_token');
    const expDate = localStorage.getItem('expiration_date');
    this._expiration = expDate ? new Date(expDate) : null;

  }

  setTokens(loginResp: LoginContract) {
    this._accessToken = loginResp.access_token || '';
    this._refreshToken = loginResp.refresh_token || '';
    const date = new Date();
    this._expiration = new Date(date.setSeconds(loginResp.expires_in)) || null;

    localStorage.setItem('access_token', this._accessToken);
    localStorage.setItem('refresh_token', this._refreshToken);
    localStorage.setItem('expiration_date', this._expiration ? this._expiration.toString() : '');
  }
}
