import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from './components/text-input/text-input.component';
import { ProfileEditComponent } from './components/profile-edit/profile-edit.component';
import { CoreModule } from '@app/core/core.module';

@NgModule({
  declarations: [ProfileEditComponent, TextInputComponent],
  imports: [CommonModule, CoreModule],
  exports: [ProfileEditComponent, TextInputComponent],
})
export class SharedModule {}
