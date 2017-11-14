import {Component, Inject} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';


@Component({
    selector: 'registration',
    templateUrl: 'registration.component.html',
    styleUrls: ['./registration.component.css']
  })
  export class RegistrationComponent {
  
    constructor(
      public dialogRef: MatDialogRef<RegistrationComponent>,
      @Inject(MAT_DIALOG_DATA) public data: any) { }

    sendDataRegistration(nickname:string,password:string,confiramtion:string,capcha:string):void
    {
        console.log(nickname,password,confiramtion,capcha);
    }  
  
    onNoClick(): void {
      this.dialogRef.close();
    }
}