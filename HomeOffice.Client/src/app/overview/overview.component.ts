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
import { Observable } from 'rxjs';
import { OfficeTime } from '../Interfaces/ITime';

@Component({
  selector: 'app-calendar',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss'],
})
export class OverviewComponent implements OnInit {
  public currentMonth!: Date;
  public calendarDays!: Date[][];
  public userId = 1;
  selectedDayData: any;
  entries: any[] = [];
  selectedDate: string | null = null;
  totalMinutes: number | null = null;
  errorMessage: string | null = null;

  public user: OfficeTime[];

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.currentMonth = new Date();
    this.generateCalendar();
    this.selectedDate = new Date().toISOString().split('T')[0];
    this.getAllTimeEntries();
  }

  getAllTimeEntries(): void {
    this.dataService.getAll().subscribe(
      (data: OfficeTime[]) => {
        this.user = data;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  showSpecific(): void {
    this.selectedDayData =
      this.user.filter(
        (entry) =>
          entry.userid === this.userId &&
          this.isSameDate(new Date(entry.date), new Date(this.selectedDate))
      ) || null;
    if (!this.selectedDayData) {
      this.errorMessage = 'Kein Eintrag für diesen Tag gefunden.';
    } else {
      this.errorMessage = null;
    }
  }

  onDateChange(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement && inputElement.value) {
      this.selectedDate = inputElement.value;
      this.showSpecific();
    }
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

  isSameDate(date1: Date, date2: Date): boolean {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  }
}
