import { Component, HostListener, OnInit } from '@angular/core';
import { UserService } from '../userService';
import { MatDialog } from '@angular/material';

import { LogInComponent } from '../login/login.component';

import { Web3, web3 } from '../Web3Service';
import {PromiseService} from './promiseService';
import { Data } from '@angular/router/src/config';

@Component({
    selector: 'Make-a-Promise-created',
    templateUrl: './makeapromise.created.component.html',
    styleUrls: ['./makeapromise.created.component.css']
})



export class MakeaPromiseCreatedComponent implements OnInit {
    web3;
    isMetaMaskInstalled: boolean;
    isAuthorization: boolean;
    constructor(
        private dialog: MatDialog,
        private userService: UserService,
        private promiseService: PromiseService
    ) { }
    ngOnInit() {
        if (typeof Web3 != "undefined") {

            this.isMetaMaskInstalled = true;

        }
    }

    savePromiseOnServer(textPromise:string,dataImplementationPromise:Data,isPublish:boolean) {

        this.userService.isAuthorization().then(data => {
        this.isAuthorization = (JSON.parse(data._body) ==true);
            if (this.isAuthorization) {
                if(textPromise!=""){
                    
                    this.promiseService.savePromiseOnServer(textPromise,dataImplementationPromise,isPublish);
                }
            }
            else {
                let dialogRef = this.dialog.open(LogInComponent, {
                    width: '350px',

                });
            }
        });
        

    }
}
