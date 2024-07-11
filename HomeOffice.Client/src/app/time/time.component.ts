import { Component } from '@angular/core';
import { TimeService } from '../services/time.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-time',
  templateUrl: './time.component.html',
  styleUrls: ['./time.component.scss'],
})
export class TimeComponent {
  constructor(private timeService: TimeService) {}

  // userId: number = 1;

  public StartTime() {
    this.timeService.startTime().subscribe(
      (response) => {
        console.log('Zeit gestartet', response);
      },
      (error : HttpErrorResponse) => {
        console.error('Fehler beim Starten: ', error);
        if ( error.status == 400 ||  error.status ==500){
          console.error("Details", error.error);
        }
      }
    );
  }

  public StopTime() {
    this.timeService.stopTime().subscribe(
      (response) => {
        console.log('Zeit wurde gestoppt und abgespeichert', response);
      },
      (error) => {
        console.error('Fehler beim Stoppen: ', error);
      }
    );
  }
}
