import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SiteStateService {
  isRequestInProgress = false;

  constructor() { }
}
