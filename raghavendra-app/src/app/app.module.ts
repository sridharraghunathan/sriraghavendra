import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { environment } from '@app-environments/environment';
import { GoogleLoginProvider, SocialAuthService, SocialAuthServiceConfig } from 'angularx-social-login';

import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { AppRoutingModule } from './app-routing.module';

import { RevealDirective } from './reveal.directive';
import { TokenInterceptor } from './core/interceptors/token.interceptor';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';

//Components
// If we use any forms module in main items then we need to register the component here.

import { AppComponent } from './app.component';
import { GoogleSigninComponent } from './accounts/google-signin/google-signin.component';
import { NavBarComponent } from './main/nav-bar/nav-bar.component';
import { HomeComponent } from './main/home/home.component';
import { ContactComponent } from './main/contact/contact.component';
import { SevaComponent } from './main/seva/seva.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AboutComponent } from './main/about/about.component';
import { GalleryComponent } from './main/gallery/gallery.component';

@NgModule({
  declarations: [
    AppComponent,
    GoogleSigninComponent,
    RevealDirective,
    NavBarComponent,
    SevaComponent,
    HomeComponent,
    ContactComponent,
    AboutComponent,
    GalleryComponent,
  ],
  imports: [AppRoutingModule, BrowserModule, BrowserAnimationsModule, HttpClientModule,
    ReactiveFormsModule, FormsModule, CoreModule, SharedModule],
  providers: [
    SocialAuthService,
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,

            provider: new GoogleLoginProvider(environment.clientId, {
              prompt: 'select_account',
            }),
          },
        ],
      } as SocialAuthServiceConfig,
    },
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
