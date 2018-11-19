import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SiteStateService {
  requestsCount: number = 0;

  constructor() { }

  get isRequestInProgress(): boolean {
    return this.requestsCount > 0;
  }
}
