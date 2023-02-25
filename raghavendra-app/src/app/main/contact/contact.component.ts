import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InfoService } from '@app-services/info.service';
import { UserQuery } from '@app/models/IUser';
import { CustomValidation } from '@app/shared/custom-validation';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
})
export class ContactComponent implements OnInit {
  contactForm: FormGroup;
  subscription = new Subscription();
  templeInfo = {};

  constructor(private fb: FormBuilder, private infoService: InfoService, private toastrService: ToastrService) {}

  ngOnInit(): void {
    this.QuestionForm();
    
    this.subscription = this.infoService.getTempleContactInfo().subscribe((data) => (this.templeInfo = data));
  }

  QuestionForm() {
    this.contactForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]],
      mobileNumber: ['', Validators.required],
      additionalInformation: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.contactForm.valid) {
      const userQuery = new UserQuery();
      userQuery.email = this.contactForm.get('email').value;
      userQuery.fullName = this.contactForm.get('fullName').value;
      userQuery.mobileNumber = this.contactForm.get('mobileNumber').value;
      userQuery.question = this.contactForm.get('additionalInformation').value;

      this.subscription = this.infoService.userClarfication(userQuery).subscribe((status) => {
        status.statusCode === 0 ? this.toastrService.success(status.messageInfo) : this.toastrService.error(status.messageInfo);
        this.contactForm.reset();
      });
    } else {
      CustomValidation.validateAllFormFields(this.contactForm);
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
