import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { StoreHeaders } from '../storeHeaders';
import { StoreLinks } from '../storeLinks';
import { Data } from '@angular/router/src/config';
import { Router } from '@angular/router';
import { promiseData } from './promiseData';





@Injectable()
export class PromiseService {
    // URL to web api
    constructor(
        private http: Http,
        private _storeHeaders: StoreHeaders,
        private _storeLinks: StoreLinks,
        private _router:Router
    ) { }
    getPromise(promiseId:string):Promise<any>{
        return this.http.get(
            this._storeLinks.SavePromiseOnServerUrl+"/"+promiseId,
            { headers: this._storeHeaders.jsonAndTokenHeaders }
        )
            .toPromise()
            .then(resp => { return resp })
            .catch(this.handleError);
    }

    savePromiseOnServer(text: string, dueDate: DateTimeFormatPart, isPublic: boolean): void {
        var data = {
            dueDate,
            isPublic,
            text
        };
        var test3 =dueDate;
        debugger
        var test ;
        this.http.post(
            this._storeLinks.SavePromiseOnServerUrl,
            data,
            { headers: this._storeHeaders.tokenHeaders }
        )
            .toPromise()
            .then(resp => { 
                debugger; 
                test=resp.text();
                debugger;
                test.Id
                promiseData.push(test);
                var testLenght = promiseData.length; 
        debugger;
        var test2 = promiseData.slice(promiseData.length-1,promiseData.length);
        
                this._router.navigateByUrl('/Make-a-Promise-Success'); })
            .catch(this.handleError);
    }
    private handleError(error: any): Promise<any> {

        return Promise.reject(error);
      }
}