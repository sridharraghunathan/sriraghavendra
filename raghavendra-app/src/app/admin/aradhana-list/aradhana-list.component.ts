import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from '@app-admin/admin.service';
import { Email, EmailFlow } from '@app/models/Email';
import { IUserInfo } from '@app/models/IUserInfo';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-aradhana-list',
  templateUrl: './aradhana-list.component.html',
  styleUrls: ['./aradhana-list.component.scss'],
})
export class AradhanaListComponent implements OnInit {
  constructor(
    private adminService: AdminService,
    private toastrService: ToastrService,
    private activatedroute: ActivatedRoute,
    private router: Router
  ) {}
  userData$: Observable<IUserInfo[]>;
  selectedUser: IUserInfo;
  isFormActive = false;
  subscription = new Subscription();

  searchObject = {
    category: 'name',
    value: '',
  };

  categories: any = [
    { key: 'FullName', value: 'name' },
    { key: 'Email Id', value: 'email' },
    { key: 'Mobile Number', value: 'phone' },
    { key: 'Year', value: 'year' },
    { key: 'City', value: 'city' },
  ];

  ngOnInit(): void {
    //default Request call for table binding
    this.userData$ = this.adminService.getListByCategory('name', 'sri', 'aradhana');
  }

  onSelectedCategory(event: string) {
    this.searchObject.category = event;
  }

  //Search event triggered by the user.
  onSearch() {
    this.userData$ = this.adminService.getListByCategory(this.searchObject.category, this.searchObject.value, 'aradhana');
  }

  // on Selection of Send email button
  onSend(user: IUserInfo) {
    this.selectedUser = user;
    this.isFormActive = true;
  }

  //Http response to the server for the Aradhana acknowledgement
  onSendEmail(user: NgForm) {
    const email = new Email();
    email.FullName = this.selectedUser.fullName;
    email.SevaName = this.selectedUser.sevaName;
    email.Address = this.selectedUser.address;
    email.AmountReceived = this.selectedUser.amount;
    email.To.push(this.selectedUser.email);
    email.Type = EmailFlow.AradhanaAmountReceived;
    email.FromEmail = user.value.fromemail;
    email.Password = user.value.password;
    this.subscription = this.adminService.sendEmail(email).subscribe((status) => {
      status.statusCode === 0 ? this.toastrService.success(status.messageInfo) : this.toastrService.error(status.messageInfo);
      this.isFormActive = false;
    });
  }
  onclose() {
    this.isFormActive = false;
  }

  //Passing the router paramter and using the activated route to get the latest router url and appending the path
  // along with it also we have used the navigation params for the navigation to the Profile COmponent
  onNavigate(user: IUserInfo) {
    this.router.navigate(['view', user.id], { relativeTo: this.activatedroute, state: { user, access: 'view' } });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
