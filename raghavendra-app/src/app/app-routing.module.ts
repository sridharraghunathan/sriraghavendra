import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';
import { NotFoundComponent } from '@app-corecmp/not-found/not-found.component';
import { AdminGuard } from './core/admin.guard';
import { AboutComponent } from './main/about/about.component';
import { ContactComponent } from './main/contact/contact.component';
import { GalleryComponent } from './main/gallery/gallery.component';
import { HomeComponent } from './main/home/home.component';
import { SevaComponent } from './main/seva/seva.component';
import { ProfileEditComponent } from './shared/components/profile-edit/profile-edit.component';

/// For scrolling using Fragments in the same page 
const routerOptions: ExtraOptions = {
  scrollPositionRestoration: "enabled",
  anchorScrolling: "enabled",
  scrollOffset: [0, 64]
};

const routes: Routes = [
  { path: 'home', component: HomeComponent, data: { title: 'Home Page - Sri Raghavendra Swamy Temple Dharmapuri' } },
  { path: 'about', component: AboutComponent, data: { title: 'About - Sri Raghavendra Swamy Temple Dharmapuri' } },
  { path: 'seva', component: SevaComponent, data: { title: 'Seva List | Donation - Sri Raghavendra Swamy Temple Dharmapuri' } },
  { path: 'contacts', component: ContactComponent, data: { title: 'Contact Details | Location or contacts of - Sri Raghavendra Swamy Temple Dharmapuri' } },
  { path: 'gallery', component: GalleryComponent, data: { title: 'Images | Events happened at Sri Raghavendra Swamy Temple Dharmapuri' } },
  { path: 'donation', component: ProfileEditComponent },
  { path: 'not-found', component: NotFoundComponent },
  {
    path: 'admin',
    canLoad: [AdminGuard],
    loadChildren: () => import('./admin/admin.module').then((m) => m.AdminModule),
  },
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes,routerOptions)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
