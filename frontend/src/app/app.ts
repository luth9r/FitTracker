import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {

  private translate = inject(TranslateService);
  private cookieService = inject(CookieService); 

  ngOnInit(): void {
    this.setupLanguage();
  }

  setupLanguage(): void {
    const supportedLangs = ['en', 'uk'];
    
    // Add supported languages
    this.translate.addLangs(supportedLangs);
    this.translate.setDefaultLang('en');

    // Try to get stored language
    const storedLang = localStorage.getItem('user_lang');
    console.log('Stored lang:', storedLang);
    
    if (storedLang && supportedLangs.includes(storedLang)) {
      this.loadLanguage(storedLang);
      return;
    }

    // Try to use browser language
    const browserLang = this.translate.getBrowserLang();
    console.log('Browser lang:', browserLang);
    
    if (browserLang && supportedLangs.includes(browserLang)) {
      this.loadLanguage(browserLang);
    } else {
      // Default to English
      this.loadLanguage('en');
    }
  }

  private loadLanguage(lang: string): void {
    console.log(`Attempting to load language: ${lang}`);
    this.translate.use(lang).subscribe({
      next: () => {
        console.log(`✓ Successfully loaded language: ${lang}`);
      },
      error: (error) => {
        console.error(`✗ Error loading language ${lang}:`, error);
        // Fallback to English
        if (lang !== 'en') {
          console.log('Falling back to English...');
          this.loadLanguage('en');
        }
      }
    });
  }
}
