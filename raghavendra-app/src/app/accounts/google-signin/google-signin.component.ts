import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { GoogleSigninService } from '../google-signin.service';

@Component({
  selector: 'app-google-signin',
  templateUrl: './google-signin.component.html',
  styleUrls: ['./google-signin.component.scss'],
})
export class GoogleSigninComponent implements OnInit {
  isLoggedin: boolean = false;
  subscription = new Subscription();
  @Output() close = new EventEmitter<boolean>();

  constructor(private router: Router, private googleSignInService: GoogleSigninService) {}

  ngOnInit() {
    this.subscription = this.googleSignInService.userInfo$.subscribe((user) => {
      if (user && user?.isAdmin) {
        this.router.navigate(['admin']);
      } else {
        this.router.navigate(['home']);
      }
    });
  }

  loginWithGoogle(): void {
    this.googleSignInService.loginWithGoogle();
  }

  onClose() {
    this.close.emit(false);
  }

  logOut(): void {
    this.googleSignInService.logOut();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
