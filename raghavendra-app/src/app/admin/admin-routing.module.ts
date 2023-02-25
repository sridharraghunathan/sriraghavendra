import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { ContactListComponent } from './contact-list/contact-list.component';
import { AradhanaListComponent } from './aradhana-list/aradhana-list.component';
import { ProfileEditComponent } from '../shared/components/profile-edit/profile-edit.component';
import { LatestUpdateComponent } from './latest-update/latest-update.component';
import { ClarificationComponent } from './clarification/clarification.component';
import { PreventUnsavedChangesGuard } from '@app/core/prevent-unsaved-changes.guard';
import { UploadPhotoComponent } from './upload-photo/upload-photo.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        redirectTo: 'contact',
      },
      {
        path: 'contact',
        component: ContactListComponent,
      },
      {
        path: 'contact/new',
        canDeactivate: [PreventUnsavedChangesGuard],
        component: ProfileEditComponent,
      },
      {
        path: 'contact/edit/:id',
        canDeactivate: [PreventUnsavedChangesGuard],
        component: ProfileEditComponent,
      },
      {
        path: 'aradhana',
        component: AradhanaListComponent,
      },
      {
        path: 'aradhana/new',
        canDeactivate: [PreventUnsavedChangesGuard],
        component: ProfileEditComponent,
      },
      {
        path: 'aradhana/view/:id',
        component: ProfileEditComponent,
      },
      {
        path: 'latest-events',
        canDeactivate: [PreventUnsavedChangesGuard],
        component: LatestUpdateComponent,
      },
      {
        path: 'upload-photo',
        canDeactivate: [PreventUnsavedChangesGuard],
        component: UploadPhotoComponent,
      },
      {
        path: 'clarification',
        component: ClarificationComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
