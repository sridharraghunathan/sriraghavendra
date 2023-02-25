import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalStorageService } from '@app-services/localstorage.service';
import { ProfileService } from '@app-services/profile.service';
import { IBaseComponent } from '@app/core/base.component';
import { globaldata } from '@app/models/constants';
import { ISevaList } from '@app/models/IUser';
import { IUserInfo } from '@app/models/IUserInfo';
import { CustomValidation } from '@app/shared/custom-validation';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.scss'],
})
export class ProfileEditComponent implements OnInit, IBaseComponent {
  consolidatedList: ISevaList[] = [];
  getTotal = 1;
  contactForm: FormGroup;
  id: string;
  userInfo: IUserInfo;
  access = false;
  modificationType: string = 'new';
  componentType: string;
  subscription = new Subscription();
  isAdmin = false;
  formSubmitted: boolean = false;
  isFormValid = () => this.contactForm.dirty;
  // isFormValid = () => this.formSubmitted || this.contactForm.dirty;

  /// this is enabled when moving to another page in the browser not another url same serverl
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.contactForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private profileService: ProfileService,
    private toastrService: ToastrService,
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private localStorage: LocalStorageService
  ) {
    const navigation = this.router.getCurrentNavigation();
    // Navigation extras from the ardhana list or contact list component
    // to identify the permission to the view or modify the data
    const state = navigation && navigation.extras && navigation.extras.state;

    if (state?.user) {
      this.userInfo = state.user as IUserInfo;
      this.access = state.access === 'view' ? true : false;
      this.modificationType = state?.access != null ? 'edit' : 'new';
    } else if (state?.selectedSevaList) {
      this.consolidatedList = state.selectedSevaList;
      this.getTotal = state.total;
    }
  }

  ngOnInit(): void {
    // Future Logic needs to be constructed for showing the Selected Items to User not for Admin
    this.isAdmin = this.localStorage.getLocalStorageByKey(globaldata.userData)?.isAdmin;
    // Component to show the button based on the router
    this.componentType = this.router.url.includes('aradhana') || this.router.url.includes('donation') ? 'aradhana' : 'contact';
    this.createLoginForm();
    // Getting the Identifier to Update the data to API CALL
    this.subscription = this.activatedRoute.params.subscribe((params) => {
      this.id = params['id'];
    });

    // Extracting the user information and patching the same to the Form model and
    // used the object extraction to get only user object
    if (this.id && this.userInfo) {
      const { id, year, ...userObject } = this.userInfo;
      this.contactForm.patchValue(userObject);
    }
  }

  // FormIntializtion for the LOGIN
  createLoginForm() {
    this.contactForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]],
      mobileNumber: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      pincode: ['', Validators.required],
      country: [''],
      additionalInformation: [''],
      sevaName: [''],
      amount: [''],
    });
  }

  // Form Submission
  onSubmit() {
    if (this.contactForm.valid) {
      if (this.modificationType === 'new') {
        const sevaName = this.consolidatedList.length > 0 ? this.consolidatedList.map((el) => el.sevaName).join(', ') : '';
        const contactForm = { ...this.contactForm.value, sevaName, amount: this.getTotal };
        this.formSubmitted = true;
        this.profileService.createContact(contactForm, this.componentType).subscribe((status) => {
          status.statusCode === 0 ? this.toastrService.success(status.messageInfo) : this.toastrService.error(status.messageInfo);
          this.resetForm();
        });
      } else if (this.modificationType === 'edit') {
        this.profileService.updateContact(this.userInfo.id, this.contactForm.value).subscribe((status) => {
          status.statusCode === 0 ? this.toastrService.success(status.messageInfo) : this.toastrService.error(status.messageInfo);
          this.resetForm();
        });
      }
    } else {
      CustomValidation.validateAllFormFields(this.contactForm);
    }
  }

  resetForm() {
    this.contactForm.reset();
    this.consolidatedList = [];
    this.getTotal = null;
    this.formSubmitted = false;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
