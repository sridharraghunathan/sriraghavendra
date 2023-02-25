import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { InfoService } from '@app-services/info.service';
import { ISevaList } from '@app/models/IUser';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-seva',
  templateUrl: './seva.component.html',
  styleUrls: ['./seva.component.scss'],
})
export class SevaComponent implements OnInit {
  constructor(private infoService: InfoService, private router: Router, private elementRef: ElementRef, private render: Renderer2) {}
  sevaList$: Observable<ISevaList[]>;
  selectedSevaList: ISevaList[] = [];
  selectedDonationList: ISevaList[] = [];
  consolidatedList: ISevaList[] = [];
  getTotal: number;
  isInitialStatus = true;
  isDisabled = true;

  ngOnInit(): void {
    this.sevaList$ = this.infoService.getSevaList();
  }

  //Seva Selection Method
  onSelection(seva: ISevaList, event: any) {
    this.consolidatedList = [];
    if (this.selectedSevaList.includes(seva)) {
      this.selectedSevaList.splice(
        this.selectedSevaList.findIndex((s) => s.sevaName === seva.sevaName),
        1
      );

      this.render.removeClass(event.currentTarget, 'active-selection');
      this.render.removeClass(event.currentTarget.children[0].children[0], 'active');
    } else {  
      this.selectedSevaList.push(seva);
      this.render.addClass(event.currentTarget, 'active-selection');
      this.render.addClass(event.currentTarget.children[0].children[0], 'active');
    }

    this.getTotal = this.selectedSevaList.map((el) => el.amount).reduce((prev, curr) => prev + curr, 0);
    if (this.selectedDonationList?.length > 0) {
      this.consolidatedList = [...this.selectedSevaList, ...this.selectedDonationList];
    } else {
      this.consolidatedList = this.selectedSevaList;
    }
    this.changeDisabledState();
  }

  //Donation Value Entered
  onAmountEntered(donationForm: NgForm) {
    const donation = donationForm.value;
    this.selectedDonationList = [];
    this.consolidatedList = [];
    Object.keys(donation).forEach((key: string, value: any) => {
      if (donation[key]) {
        this.selectedDonationList.push({ sevaName: key, description: '', amount: +donation[key] });
      }
    });

    if (this.selectedSevaList?.length > 0) {
      this.consolidatedList = [...this.selectedSevaList, ...this.selectedDonationList];
    } else {
      this.consolidatedList = this.selectedDonationList;
      this.getTotal = this.consolidatedList.map((el) => el.amount).reduce((prev, curr) => prev + curr, 0);
    }
    this.changeDisabledState();
  }

  // Clearing the Selection
  onReset() {
    const element = this.elementRef.nativeElement.querySelectorAll('.active-selection');
    element.forEach((element) => {
      this.render.removeClass(element, 'active-selection');
    });
    this.selectedSevaList = [];
    this.selectedDonationList = [];
    this.consolidatedList = [];
    this.getTotal = 0;
    this.changeDisabledState();
  }

  changeDisabledState() {
    this.isDisabled = this.consolidatedList.length <= 0 ? true : false;
  }

  onProceed() {
    this.router.navigate(['donation'], { state: { selectedSevaList: this.consolidatedList, total: this.getTotal } });
  }
}
