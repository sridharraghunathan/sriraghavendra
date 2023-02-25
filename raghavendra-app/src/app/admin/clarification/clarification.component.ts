import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AdminService } from '@app-admin/admin.service';
import { IUserQuery, UserQueryEmail } from '@app/models/IUser';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-clarification',
  templateUrl: './clarification.component.html',
  styleUrls: ['./clarification.component.scss'],
})
export class ClarificationComponent implements OnInit {
  userClarification$: Observable<IUserQuery[]>;
  selectedUser: IUserQuery;
  isFormActive = false;
  subscription = new Subscription();

  constructor(private adminService: AdminService, private toastrService: ToastrService) {}

  ngOnInit(): void {
    this.userClarification$ = this.adminService.getUserClarification();
  }

  onSendEmail(user: IUserQuery) {
    this.selectedUser = user;
    this.isFormActive = true;
  }

  onclose() {
    this.isFormActive = false;
  }

  //Http Request for the sending the email to the user
  onSubmit(contactForm: NgForm) {
    if (contactForm.valid) {
      const userQuery = new UserQueryEmail();
      userQuery.FullName = this.selectedUser.fullName;
      userQuery.To.push(this.selectedUser.email);
      userQuery.Question = this.selectedUser.question;
      userQuery.Reply = contactForm.value.body;
      userQuery.FromEmail = contactForm.value.fromemail;
      userQuery.Password = contactForm.value.password;
      this.subscription = this.adminService.userClarfication(userQuery).subscribe((status) => {
        status.statusCode === 0 ? this.toastrService.success(status.messageInfo) : this.toastrService.error(status.messageInfo);
        this.isFormActive = false;
      });
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
