import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FooterComponent } from './components/footer/footer.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ToastrModule } from 'ngx-toastr';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ScrollToTopComponent } from './components/scroll-to-top/scroll-to-top.component';

@NgModule({
  declarations: [FooterComponent, NotFoundComponent, ScrollToTopComponent],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass:'toast-top-right',
      preventDuplicates: true,
    }),
  ],
  exports: [FooterComponent, ToastrModule
    , ReactiveFormsModule, FormsModule,ScrollToTopComponent
  ],
})
export class CoreModule {}
