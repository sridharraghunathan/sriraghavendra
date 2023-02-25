import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyCount = 0;
  spinnerState = new Subject<boolean>();
  spinnerState$ = this.spinnerState.asObservable();


  /// Loading Service for the spinner
  busy(): void {
    this.busyCount++;
    this.spinnerState.next(true);
  }

  idle(): void {
    this.busyCount--;
    if (this.busyCount <= 0) {
      this.busyCount = 0;
    }
    this.spinnerState.next(false);
  }
}
