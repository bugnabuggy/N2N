import { async, ComponentFixture, fakeAsync, inject, TestBed, tick} from '@angular/core/testing';
import { MakeaPromiseCreatedComponent } from './makeapromise.created.component';
import { RouterTestingModule } from '@angular/router/testing'

import { HttpModule } from '@angular/http';

import {Web3, Web3Service,web3} from '../Web3Service';


import { By }           from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

// describe('MakeaPromiseComponent', () => {
    
// });
// beforeEach(async(() => {
//     TestBed.configureTestingModule({
//         declarations: [
//             MakeaPromiseComponent
//           ],
//           imports: [
//             RouterTestingModule,
//             HttpModule 
//           ],
//         providers:[ Web3, Web3Service,web3]
//     }).compileComponents();
// }));

// it('detected or not MetaMask',inject([Web3], (Web3)=>{
//     expect(Web3).toBe('undefined');
// }));