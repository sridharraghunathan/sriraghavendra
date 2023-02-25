import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AdminService } from '@app-admin/admin.service';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-upload-photo',
  templateUrl: './upload-photo.component.html',
  styleUrls: ['./upload-photo.component.scss'],
})
export class UploadPhotoComponent implements OnInit {
  eventForm: FormGroup;
  fileToUpload: File = null;
  subscription = new Subscription();
  formSubmitted: boolean = false;
  isFormValid = () => this.eventForm.dirty;

  constructor(private fb: FormBuilder, private toastrService: ToastrService, private adminService: AdminService) {}

  ngOnInit(): void {
    this.latesteventsForm();
  }

  latesteventsForm() {
    this.eventForm = this.fb.group({
      title: ['', Validators.required],
      image: [''],
      eventDate: ['', Validators.required],
    });
  }

  // setControl(fileToUpload: string): void {
  //   this.eventForm.get('image')?.setValue(fileToUpload);
  // }

  // Upload File method to Upload the data to backend which will be send back the Cloudinary Account
  uploadFile = (files: File[]) => {
    if (files.length === 0) {
      return;
    }
    this.fileToUpload = files[0] as File;
    // this.setControl(this.fileToUpload.name);
  };

  //Http Request for posting the Upcoming Events
  onSubmit() {
    const formData = new FormData();
    formData.append('title', this.eventForm.get('title').value);
    formData.append('eventDate', this.eventForm.get('eventDate').value);
    if (this.fileToUpload) {
      formData.append('image', this.fileToUpload, this.fileToUpload.name);
    }
    this.formSubmitted = true;
    this.subscription = this.adminService.uploadingPhoto(formData).subscribe((status) => {
      this.resetForm();
      status.statusCode === 0 ? this.toastrService.success(status.messageInfo) : this.toastrService.error(status.messageInfo);
    });
  }

  resetForm() {
    this.eventForm.reset();
    this.formSubmitted = false;
    this.fileToUpload = null;
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
