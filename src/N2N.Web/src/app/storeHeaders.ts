import { Headers, Http } from '@angular/http';


export class StoreHeaders {

    public jsonHeader = new Headers({
        'Content-Type': 'application/json charset=utf-8',
      });
      
      public jsonAndTokenHeaders = new Headers({
        'Content-Type': 'application/json charset=utf-8',
        "Authorization": 'Bearer ' + localStorage.getItem('Token')
      });

      public refrechJsonAndTokenHeaders(){
        debugger; 
          this.jsonAndTokenHeaders=new Headers({
            'Content-Type': 'application/json charset=utf-8',
            "Authorization": 'Bearer ' + localStorage.getItem('Token')
          });
      }
}