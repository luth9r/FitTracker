import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {

  constructor(private translate: TranslateService) {
    this.setupLanguage();
  }

  setupLanguage(): void {
    const supportedLangs = ['en', 'uk'];
    
    this.translate.addLangs(supportedLangs);
    this.translate.setDefaultLang('en');

    const storedLang = localStorage.getItem('user_lang');
    if (storedLang && supportedLangs.includes(storedLang)) {
      this.translate.use(storedLang);
      return;
    }


    const browserLang = this.translate.getBrowserLang();
    if (browserLang && supportedLangs.includes(browserLang)) {
      this.translate.use(browserLang); 
    } else {
      this.translate.use('en');
    }
  }
}
