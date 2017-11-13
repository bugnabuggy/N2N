import {Component, Inject} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';

import {RegistrationComponent} from '../registration/registration.component';
@Component({
    selector: 'login',
    templateUrl: 'login.component.html',
    styleUrls: ['./login.component.css']
  })
  export class LogInComponent {
  
    constructor(
      public dialog: MatDialog,
      public dialogRef: MatDialogRef<LogInComponent>,
      @Inject(MAT_DIALOG_DATA) public data: any) { }

    sendDataLogIn(nickname:string,password:string,capcha:string):void
    {
      
      console.log(nickname,password);
    }  
      
    onNoClick(): void {
      this.dialogRef.close();
    }

    openDialog(): void {
      this.dialogRef.close();
      let dialogRef = this.dialog.open(RegistrationComponent, {
        width: '350px',
        
      });
      
  }
}