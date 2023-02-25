import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule } from './admin-routing.module';

import { LatestUpdateComponent } from './latest-update/latest-update.component';
import { ContactListComponent } from './contact-list/contact-list.component';
import { AradhanaListComponent } from './aradhana-list/aradhana-list.component';
import { CoreModule } from '@app/core/core.module';
import { SharedModule } from '@app/shared/shared.module';
import { ClarificationComponent } from './clarification/clarification.component';
import { UploadPhotoComponent } from './upload-photo/upload-photo.component';

@NgModule({
  declarations: [AdminComponent, LatestUpdateComponent, ContactListComponent, AradhanaListComponent, ClarificationComponent, UploadPhotoComponent],
  imports: [CommonModule, AdminRoutingModule, CoreModule, SharedModule],
})
export class AdminModule {}
