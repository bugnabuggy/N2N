import { Component, HostListener, OnInit } from '@angular/core';
import { UserService } from '../userService';
import { MatDialog } from '@angular/material';
import { Input} from '@angular/core';
import { Data } from '@angular/router/src/config';

@Component({
    selector: 'Make-a-Promise-Success',
    templateUrl: './makeapromise.success.component.html',
    styleUrls: ['./makeapromise.success.component.css']
})



export class MakeaPromiseSuccessComponent implements OnInit {
    @Input() text: string;
    @Input() date: Data;
    constructor(
        private userService: UserService
    ) { }
    ngOnInit() {
        
    }
}