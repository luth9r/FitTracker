import { Injectable, inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {
  private translate = inject(TranslateService);
  private settingsService = inject(SettingsService);

  /**
   * Converts weight from kg to current unit system
   * @param weightInKg Weight in kilograms
   * @returns Weight in current units (kg or lbs)
   */
  convertWeight(weightInKg: number): number {
    const unit = this.settingsService.getWeightUnit();
    if (unit === 'lbs') {
      return weightInKg * 2.20462;
    }
    return weightInKg;
  }

  /**
   * Gets translated unit label
   * @returns Translated string ('кг', 'kg', 'фунт', 'lbs')
   */
  getUnitLabel(): string {
    const unit = this.settingsService.getWeightUnit();
    return this.translate.instant(`COMMON.UNITS.${unit.toUpperCase()}`);
  }

  /**
   * Gets raw weight unit
   * @returns 'kg' or 'lbs'
   */
  getWeightUnit(): string {
    return this.settingsService.getWeightUnit();
  }

  /**
   * Formats date to localized string with year
   * @param dateString ISO date string or null/undefined
   * @returns Localized date or "Never"
   */
  formatDate(dateString: string | null | undefined): string {
    if (!dateString) {
      return this.translate.instant('COMMON.NEVER');
    }
    const date = new Date(dateString);
    const locale = this.getLocale();
    
    return date.toLocaleDateString(locale, { 
      day: 'numeric', 
      month: 'short', 
      year: 'numeric' 
    });
  }

  /**
   * Formats date to localized string without year
   * @param dateString ISO date string
   * @returns Localized date (day + month)
   */
  formatDateShort(dateString: string): string {
    const date = new Date(dateString);
    const locale = this.getLocale();
    
    return date.toLocaleDateString(locale, { 
      day: 'numeric', 
      month: 'short' 
    });
  }

  /**
   * Gets locale for date formatting
   * @returns Locale code (uk-UA, en-US)
   */
  private getLocale(): string {
    const currentLang = this.translate.currentLang || 'uk';
    const localeMap: Record<string, string> = {
      'uk': 'uk-UA',
      'en': 'en-US'
    };
    return localeMap[currentLang] || 'uk-UA';
  }
}
