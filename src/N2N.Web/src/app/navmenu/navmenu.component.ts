import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';

import {LogInComponent} from '../login/login.component';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    
    constructor(
        public dialog: MatDialog)
        {}
        

    openDialog(): void {
        let dialogRef = this.dialog.open(LogInComponent, {
          width: '350px',
        
        });
    }
    
}
