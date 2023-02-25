import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GoogleSigninService } from '@app-accounts/google-signin.service';
import { InfoService } from '@app-services/info.service';
import { IUser } from '@app/models/IUser';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  socialUser: Partial<IUser>;
  subscription = new Subscription();
  templeInfo ;

  status = {
    isLoggedin: false,
    loginClicked: false,
    isAdmin: false,
  };

  constructor(private googleSignInService: GoogleSigninService, private infoService: InfoService, private router: Router) {}

  ngOnInit(): void {
    //getting the user information to show in header
    this.subscription = this.googleSignInService.userInfo$.subscribe((user) => {
      user && user.isAdmin ? (this.status.isAdmin = user.isAdmin) : (this.status.isAdmin = false);
      this.status.loginClicked = false;
      this.socialUser = user;
      this.status.isLoggedin = user != null;
    });

    this.subscription = this.infoService.getTempleContactInfo().subscribe((data) => (this.templeInfo = data));
  }

  onLogin() {
    this.status.loginClicked = true;
  }

  onLogout() {
    this.googleSignInService.logOut();
    this.status.loginClicked = false;
    this.router.navigate(['home']);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
