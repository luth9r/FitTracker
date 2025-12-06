import { ApplicationConfig, APP_INITIALIZER } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { provideTranslateService, TranslateLoader } from "@ngx-translate/core";
import { languageInterceptor } from './interceptors/languageInterceptor'; 
import { Observable } from 'rxjs';

import { provideOAuthClient, OAuthService, AuthConfig } from 'angular-oauth2-oidc';

const YOUR_GOOGLE_CLIENT_ID = '719113265141-v8ckgmea9bob1nd65f4396n93o16dcqd.apps.googleusercontent.com';

// OAUTH CONFIGURATION
const googleOidcConfig: AuthConfig = {
  issuer: 'https://accounts.google.com',
  redirectUri: window.location.origin + '/assets/popup-callback.html',
  clientId: YOUR_GOOGLE_CLIENT_ID,
  scope: 'openid profile email',
  responseType: 'code',
  strictDiscoveryDocumentValidation: false,
  showDebugInformation: true,

  customQueryParams: {
    prompt: 'select_account consent',
    access_type: 'offline'
  },
};

export function initializeOAuth(oauthService: OAuthService): () => Promise<void> {
  return () => {
    oauthService.configure(googleOidcConfig);
    return oauthService.loadDiscoveryDocument()
      .then(() => {
        console.log('✅ Discovery loaded successfully');
      })
      .catch(err => {
        console.error('❌ Error loading discovery: ', err);
      });
  };
}

class CustomTranslateLoader implements TranslateLoader {
  constructor(private http: HttpClient) {}

  getTranslation(lang: string): Observable<any> {
    return this.http.get(`./assets/i18n/${lang}.json`);
  }
}

export function createTranslateLoader(http: HttpClient): TranslateLoader {
  return new CustomTranslateLoader(http);
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([
      languageInterceptor
    ])),

    provideTranslateService({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient]
      }
    }),

    provideOAuthClient(),

    // APP INITIALIZER FOR OAUTH
    {
      provide: APP_INITIALIZER,
      useFactory: initializeOAuth,
      deps: [OAuthService],
      multi: true
    }
  ]
};
