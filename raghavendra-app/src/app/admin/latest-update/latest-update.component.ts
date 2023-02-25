import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService } from '@app-admin/admin.service';
import { IBaseComponent } from '@app/core/base.component';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-latest-update',
  templateUrl: './latest-update.component.html',
  styleUrls: ['./latest-update.component.scss'],
})
export class LatestUpdateComponent implements OnInit, IBaseComponent {
  eventForm: FormGroup;
  fileToUpload: File = null;
  subscription = new Subscription();
  formSubmitted: boolean = false;
  isFormValid = () => this.eventForm.dirty;
  //isFormValid = () => this.formSubmitted || this.eventForm.dirty;

  constructor(private fb: FormBuilder, private toastrService: ToastrService, private adminService: AdminService) {}

  ngOnInit(): void {
    this.latesteventsForm();
  }

  latesteventsForm() {
    this.eventForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      image: [''],
      eventDate: ['', Validators.required],
    });
  }

  // setControl(fileToUpload: string): void {
  //   this.eventForm.get('image')?.setValue(fileToUpload);
  // }

  // Upload File method to Upload the data to backend which will be send back the Cloudinary Account
  // uploadFile = (files: File[]) => {
  //   if (files.length === 0) {
  //     return;
  //   }
  //   this.fileToUpload = files[0] as File;
  //   // this.setControl(this.fileToUpload.name);
  // };

  //Http Request for posting the Upcoming Events
  onSubmit() {
    const formData = new FormData();
    formData.append('title', this.eventForm.get('title').value);
    formData.append('description', this.eventForm.get('description').value);
    formData.append('eventDate', this.eventForm.get('eventDate').value);
    this.formSubmitted = true;
    this.subscription = this.adminService.createLatestEvents(formData).subscribe((status) => {
      this.resetForm();
      status.statusCode === 0 ? this.toastrService.success(status.messageInfo) : this.toastrService.error(status.messageInfo);
    });
  }

  resetForm() {
    this.eventForm.reset();
    this.formSubmitted = false;
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
