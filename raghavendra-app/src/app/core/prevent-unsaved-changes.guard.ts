import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ProfileEditComponent } from '@app/shared/components/profile-edit/profile-edit.component';
import { Observable } from 'rxjs';
import { IBaseComponent } from './base.component';

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesGuard implements CanDeactivate<IBaseComponent> {
  canDeactivate(component: IBaseComponent): Observable<boolean> | boolean {
    return !component.isFormValid() ? true : confirm('Save your changes otherwise your changes will be lost, still want to navigate then press ok?');
  }
}
