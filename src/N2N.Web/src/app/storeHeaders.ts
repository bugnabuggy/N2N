import { Headers, Http } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()
export class StoreHeaders {

  public jsonHeader = new Headers({
    'Content-Type': 'application/json charset=utf-8',
  });

  public jsonAndTokenHeaders = new Headers({
    'Content-Type': 'application/json charset=utf-8',
    "Authorization": 'Bearer ' + localStorage.getItem('Token')
  });
  public tokenHeaders = new Headers({
    "Authorization": 'Bearer ' + localStorage.getItem('Token')
  });

  public refrechJsonAndTokenHeaders() {
    this.jsonAndTokenHeaders = new Headers({
      'Content-Type': 'application/json charset=utf-8',
      "Authorization": 'Bearer ' + localStorage.getItem('Token')
    });
  }
}