import { Injectable } from '@angular/core';
export const Web3 = require('web3');
declare var window: any;
export const web3 =new Web3(window.web3.currentProvider);

@Injectable()
export class Web3Service {

}