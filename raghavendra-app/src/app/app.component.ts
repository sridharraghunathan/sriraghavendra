import { Component, OnInit } from '@angular/core';
import { globaldata } from './models/constants';
import { GoogleSigninService } from './accounts/google-signin.service';
import { LocalStorageService } from './services/localstorage.service';
import { BusyService } from '@app-services/busy.service';
import { Observable } from 'rxjs';
import { delay, filter } from 'rxjs/operators';
import { NavigationEnd, ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'raghavendra-app';
  constructor(
    private localstorage: LocalStorageService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
    private busyService: BusyService,
    private googleSignInService: GoogleSigninService
  ) {}
  spinnerState$: Observable<boolean>;
  // For Song to generate randomly this logic is written for 2 songs
  rndInt = Math.floor(Math.random() * 2) + 1;
  randomSong: string;
  initialPlay = 0;

  ngOnInit(): void {
    this.randomSong = `assets/audio/song${this.rndInt}.mp3`;
    this.spinnerState$ = this.busyService.spinnerState$.pipe(delay(0));
    //check whether user has logged in
    const userData = this.localstorage.getLocalStorageByKey(globaldata.userData);
    if (userData) {
      this.googleSignInService.setUserInfo(userData);
    }

    this.TitleChange();
  }

  private TitleChange() {
    this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe(() => {
      var rt = this.getChild(this.activatedRoute);
      rt.data.subscribe((data) => {
        this.titleService.setTitle(data.title);
      });
    });
  }

  // To play the music
  playMusic() {
    if (this.initialPlay === 0) {
      this.initialPlay++;
      (<HTMLAudioElement>document.getElementById('audio')).play();
    }
  }

  private getChild(activatedRoute: ActivatedRoute) {
    if (activatedRoute.firstChild) {
      return this.getChild(activatedRoute.firstChild);
    } else {
      return activatedRoute;
    }
  }
}
