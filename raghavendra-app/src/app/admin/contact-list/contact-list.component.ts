import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from '@app-admin/admin.service';
import { IUserInfo } from '@app/models/IUserInfo';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss'],
})
export class ContactListComponent implements OnInit {
  constructor(private adminService: AdminService, private activatedroute: ActivatedRoute, private router: Router) {}
  userData$: Observable<IUserInfo[]>;
  searchObject = {
    category: 'name',
    value: '',
  };

  categories: any = [
    { key: 'FullName', value: 'name' },
    { key: 'Email Id', value: 'email' },
    { key: 'Mobile Number', value: 'phone' },
    { key: 'City', value: 'city' },
  ];

  ngOnInit(): void {
    this.userData$ = this.adminService.getListByCategory('name', 'sri', 'contact');
  }
  onSelectedCategory(event: string) {
    this.searchObject.category = event;
  }
  onSearch() {
    this.userData$ = this.adminService.getListByCategory(this.searchObject.category, this.searchObject.value, 'contact');
  }

  //Passing the Navigation Extras to the Profile Edit component
  // When View Mode to view the data in readonly format or edit only format 

  onNavigate(user: IUserInfo, access:string) {
    this.router.navigate(['edit', user.id], { relativeTo: this.activatedroute, state: { user, access } });
  }
}
