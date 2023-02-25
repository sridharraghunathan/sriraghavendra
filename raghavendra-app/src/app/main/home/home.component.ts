import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { InfoService } from '@app-services/info.service';
import { ILatestEventsDtl } from '@app/models/IUser';
import { Observable } from 'rxjs';
import { map} from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  // @ViewChildren('animationelement', { read: ElementRef }) animationelement: QueryList<ElementRef>;
  private observer: IntersectionObserver;
  eventInfo$: Observable<ILatestEventsDtl[]>;
  sinceConstruction = new Date().getFullYear() - 1995;

  constructor(private infoService: InfoService, private router: Router) {}

  ngOnInit(): void {
    //Taking the 5 items in an array
    this.eventInfo$ = this.infoService.getLatestEvents().pipe(map((el) => el.slice(0, 5)));
  }

  onSeeAll() {
    this.eventInfo$ = this.infoService.getLatestEvents();
  }

  onDonate() {
    this.router.navigate(['seva']);
  }

  /// Showing the ANimation code has been changed as directive.
  // revealElement() {
  // elementList: any[];
  //   this.elementList = this.animationelement.map((el) => el.nativeElement);
  //   this.renderer.listen('window', 'scroll', () => {
  //     for (var i = 0; i < this.elementList.length; i++) {
  //       var windowheight = window.innerHeight;
  //       var revealtop = this.elementList[i].getBoundingClientRect().top;
  //       var revealpoint = 150;
  //       if (revealtop < windowheight - revealpoint) {
  //         this.elementList[i].classList.add('with-animation');
  //       }
  //       // else {
  //       //   this.elementList[i].classList.remove('with-animation');
  //       // }
  //     }
  //   });
  // }
}
