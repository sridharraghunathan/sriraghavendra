import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { GoogleSigninService } from '@app-accounts/google-signin.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private googleSignInService: GoogleSigninService, private toastrService: ToastrService, private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    ///Checking the currently logged in user is Admin or not using token
    return this.googleSignInService.userInfo$.pipe(
      map((response) => {
        if (response?.isAdmin) {
          return true;
        } else {
          this.toastrService.error('Only Admin can access this screen, Login as Admin!!!');
          return false;
          //  this.router.navigateByUrl('home');
        }
      })
    );
  }
}
