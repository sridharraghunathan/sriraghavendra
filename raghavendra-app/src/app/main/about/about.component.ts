import { Component, OnInit } from '@angular/core';
import { InfoService } from '@app-services/info.service';
import { IAboutTemple } from '@app/models/IUser';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss'],
})
export class AboutComponent implements OnInit {
  constructor(private infoService: InfoService) {}
  info$: Observable<IAboutTemple[]>;
  ngOnInit(): void {
    this.info$ = this.infoService.getAboutTemple();
  }
}
