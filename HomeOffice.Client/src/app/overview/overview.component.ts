import { Component, OnInit } from '@angular/core';
import {
  getDaysInMonth,
  startOfMonth,
  addDays,
  startOfWeek,
  endOfWeek,
  isSameMonth,
  format,
  addMonths,
  subMonths,
  endOfMonth,
} from 'date-fns';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-calendar',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss'],
})
export class OverviewComponent implements OnInit {
  public currentMonth!: Date;
  public calendarDays!: Date[][];
  public userId : number | undefined;
  selectedDayData: any;

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.currentMonth = new Date();
    this.generateCalendar();
    
  }

  showDayDetails(day: Date): void {
    // const formattedDate = format(day, 'yyyy-MM-dd');
    this.dataService.getHomeOfficeData().subscribe(
      (data) => {
        this.selectedDayData = data;
        console.log('Daten für Tag:', day, data);
      },
      (error) => {
        console.error('Fehler beim Laden der Daten:', error);
      }
    );
  }

  generateCalendar(): void {
    const daysInMonth = getDaysInMonth(this.currentMonth);
    const startOfMonthDate = startOfMonth(this.currentMonth);
    const endOfMonthDate = addDays(endOfWeek(endOfMonth(this.currentMonth)), 1); // Korrektur hier

    let currentDate = startOfWeek(startOfMonthDate);
    const days: Date[] = [];

    while (currentDate < endOfMonthDate) {
      days.push(currentDate);
      currentDate = addDays(currentDate, 1);
    }

    this.calendarDays = [];
    for (let i = 0; i < Math.ceil(days.length / 7); i++) {
      this.calendarDays.push(days.slice(i * 7, (i + 1) * 7));
    }
  }

  nextMonth(): void {
    this.currentMonth = addMonths(this.currentMonth, 1);
    this.generateCalendar();
  }

  previousMonth(): void {
    this.currentMonth = subMonths(this.currentMonth, 1);
    this.generateCalendar();
  }

  isSameMonth(date: Date, month: Date): boolean {
    return isSameMonth(date, month);
  }
}
