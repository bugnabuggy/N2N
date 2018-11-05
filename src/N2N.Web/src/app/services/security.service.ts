import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  private _accessToken = '';
  private _refreshToken = '';

  get accessToken() { return this._accessToken; }
  get refreshToken() { return this._refreshToken; }

  constructor() { }

  setTokens() {

  }
}
