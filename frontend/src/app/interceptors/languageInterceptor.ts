import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

/**
 * This interceptor automatically adds the 'Accept-Language' header
 * to all outgoing HTTP requests.
 * The C# backend (via 'AcceptLanguageHeaderRequestCultureProvider')
 * will see this header and automatically switch
 * the 'ILocalizationService' to the correct language.
 */
export const languageInterceptor: HttpInterceptorFn = (req, next) => {
  
  console.log('%c[Language Interceptor] Running...', 'color: #7A56D1; font-weight: bold;');

  const translateService = inject(TranslateService);
  const currentLang = translateService.currentLang || translateService.defaultLang || 'en';
  const cultureMap: { [key: string]: string } = {
    'en': 'en-US',
    'uk': 'uk-UA'
  };

  const acceptLanguageHeader = cultureMap[currentLang] || 'en-US';
  
  console.log(`%c[Language Interceptor] Current lang is: '${currentLang}'. Sending header: '${acceptLanguageHeader}'`, 'color: #7A56D1;');
  
  const clonedReq = req.clone({
    headers: req.headers.set('Accept-Language', acceptLanguageHeader)
  });

  return next(clonedReq);
};