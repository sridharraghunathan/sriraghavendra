import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SocialUser, GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { globaldata } from '../models/constants';
import { IUser } from '../models/IUser';
import { LocalStorageService } from '../services/localstorage.service';
import { environment } from '@app-environments/environment';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root',
})
export class GoogleSigninService {
  API_URL = environment.apiUrl;
  socialUser: SocialUser;
  private userInfo = new BehaviorSubject<IUser>(null);
  userInfo$ = this.userInfo.asObservable();

  constructor(
    private socialAuthService: SocialAuthService,
    private route: Router,
    private localStorage: LocalStorageService,
    private toastrService: ToastrService,
    private http: HttpClient
  ) {}

  //Google Login Using the Google Provider
  loginWithGoogle(): void {
    this.socialAuthService
      .signIn(GoogleLoginProvider.PROVIDER_ID)
      .then((user: SocialUser) => {
        //API CALL
        this.http.post(this.API_URL + 'authorization', { idToken: user.idToken }, httpOptions).subscribe((userInfo: IUser) => {
          this.localStorage.setLocalStorage(globaldata.userData, userInfo);
          this.toastrService.success(`Authenticated as ${userInfo.email}`);
          this.userInfo.next(userInfo);
          ///testing
        });
      })
      .catch(() => {
        this.cleardata();
        this.route.navigate(['home']);
      });
  }

  logOut(): void {
    const data = this.localStorage.getLocalStorageByKey(globaldata.userData);
    if (data) {
      this.socialAuthService
        .signOut()
        .then(() => {
          this.cleardata();
        })
        .catch(() => {
          this.cleardata();
        });
    }
  }

  setUserInfo = (userInfo) => this.userInfo.next(userInfo);

  cleardata = () => {
    this.localStorage.clearLocalStorage();
    this.userInfo.next(null);
  };
}
