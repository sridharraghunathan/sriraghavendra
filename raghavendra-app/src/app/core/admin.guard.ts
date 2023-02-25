import { Injectable } from '@angular/core';
import { CanLoad, Router } from '@angular/router';
import { GoogleSigninService } from '@app-accounts/google-signin.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanLoad {
  constructor(private googleSignInService: GoogleSigninService, private toastrService: ToastrService, private router: Router) {}
  canLoad(): Observable<boolean> {
    const userInfo$ = this.googleSignInService.userInfo$;
    return userInfo$.pipe(
      map((response) => {
        if (response?.isAdmin) {
          return true;
        } else {
          return this.UnAutheticatedAccesss();
        }
      })
    );
  }

  private UnAutheticatedAccesss() {
    this.toastrService.error('Only Admin can access this screen, Login as Admin!!!');
    this.router.navigateByUrl('home');
    return false;
  }
}
