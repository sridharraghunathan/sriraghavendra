import { Component, OnInit } from '@angular/core';
import { InfoService } from '@app-services/info.service';
import { IUploadPhoto } from '@app/models/IUser';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss'],
})
export class GalleryComponent implements OnInit {
  photos$: Observable<IUploadPhoto[]>;
  constructor(private infoService: InfoService) {}

  ngOnInit(): void {
    this.photos$ = this.infoService.getPhotosList();
  }
}
