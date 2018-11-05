import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(public snackBar: MatSnackBar) { }


  error(message: string) {
    this.snackBar.open(message, 'OK', {
      verticalPosition: 'top',
      panelClass: ['bg-danger', 'text-white']
    });
  }
}
