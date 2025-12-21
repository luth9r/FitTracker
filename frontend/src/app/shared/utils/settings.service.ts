import { Injectable, signal } from '@angular/core';

export type UnitSystem = 'metric' | 'imperial';
export type Language = 'uk' | 'en';

export interface UserSettings {
  unitSystem: UnitSystem;
  language: Language;
}

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private readonly LANGUAGE_STORAGE_KEY = 'user_lang';
  private readonly UNIT_SYSTEM_COOKIE_NAME = 'preferred-units';
  private readonly COOKIE_EXPIRY_DAYS = 365;

  // Default settings
  private defaultSettings: UserSettings = {
    unitSystem: 'metric',
    language: 'uk',
  };

  private settingsSignal = signal<UserSettings>(this.loadSettings());

  constructor() {
    this.settingsSignal.set(this.loadSettings());
  }

  /**
   * Loads settings from localStorage (language) and cookies (unitSystem)
   */
  private loadSettings(): UserSettings {
    const language = this.loadLanguageFromStorage();
    const unitSystem = this.loadUnitSystemFromCookie();

    return {
      language,
      unitSystem
    };
  }

  /**
   * Loads language from localStorage
   */
  private loadLanguageFromStorage(): Language {
    try {
      const stored = localStorage.getItem(this.LANGUAGE_STORAGE_KEY);
      if (stored && (stored === 'uk' || stored === 'en')) {
        return stored as Language;
      }
    } catch (error) {
      console.error('Failed to load language from localStorage:', error);
    }
    return this.defaultSettings.language;
  }

  /**
   * Loads unit system from cookies
   */
  private loadUnitSystemFromCookie(): UnitSystem {
    try {
      const cookieValue = this.getCookie(this.UNIT_SYSTEM_COOKIE_NAME);
      if (cookieValue && (cookieValue === 'metric' || cookieValue === 'imperial')) {
        return cookieValue as UnitSystem;
      }
    } catch (error) {
      console.error('Failed to load unit system from cookies:', error);
    }
    return this.defaultSettings.unitSystem;
  }

  /**
   * Saves language to localStorage
   */
  private saveLanguageToStorage(language: Language): void {
    try {
      localStorage.setItem(this.LANGUAGE_STORAGE_KEY, language);
    } catch (error) {
      console.error('Failed to save language to localStorage:', error);
    }
  }

  /**
   * Saves unit system to cookies
   */
  private saveUnitSystemToCookie(system: UnitSystem): void {
    try {
      this.setCookie(this.UNIT_SYSTEM_COOKIE_NAME, system, this.COOKIE_EXPIRY_DAYS);
    } catch (error) {
      console.error('Failed to save unit system to cookies:', error);
    }
  }

  /**
   * Gets a cookie by name
   */
  private getCookie(name: string): string | null {
    const nameEQ = name + '=';
    const cookies = document.cookie.split(';');
    
    for (let cookie of cookies) {
      cookie = cookie.trim();
      if (cookie.indexOf(nameEQ) === 0) {
        return cookie.substring(nameEQ.length);
      }
    }
    return null;
  }

  /**
   * Sets a cookie
   */
  private setCookie(name: string, value: string, days: number): void {
    const date = new Date();
    date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
    const expires = 'expires=' + date.toUTCString();
    
    document.cookie = `${name}=${value};${expires};path=/;SameSite=Strict`;
  }

  /**
   * Deletes a cookie
   */
  private deleteCookie(name: string): void {
    document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`;
  }

  /**
   * Gets all user settings
   */
  getSettings(): UserSettings {
    return this.settingsSignal();
  }

  /**
   * Gets the current unit system
   */
  getUnitSystem(): UnitSystem {
    return this.settingsSignal().unitSystem;
  }

  /**
   * Checks if using metric system
   */
  isMetric(): boolean {
    return this.settingsSignal().unitSystem === 'metric';
  }

  /**
   * Checks if using imperial system
   */
  isImperial(): boolean {
    return this.settingsSignal().unitSystem === 'imperial';
  }

  /**
   * Gets weight unit based on current system (kg or lbs)
   */
  getWeightUnit(): 'kg' | 'lbs' {
    return this.isMetric() ? 'kg' : 'lbs';
  }

  /**
   * Converts weight based on current unit system
   * @param weightKg - Weight in kilograms
   * @returns Converted weight
   */
  convertWeight(weightKg: number): number {
    return this.isMetric() ? weightKg : weightKg * 2.20462;
  }

  /**
   * Sets the unit system (saves to cookies)
   */
  setUnitSystem(system: UnitSystem): void {
    this.saveUnitSystemToCookie(system);
    const current = this.settingsSignal();
    this.settingsSignal.set({ ...current, unitSystem: system });
  }

  /**
   * Gets the current language
   */
  getLanguage(): Language {
    return this.settingsSignal().language;
  }

  /**
   * Sets the language (saves to localStorage)
   */
  setLanguage(language: Language): void {
    this.saveLanguageToStorage(language);
    const current = this.settingsSignal();
    this.settingsSignal.set({ ...current, language });
  }

  /**
   * Resets settings to default values
   */
  resetSettings(): void {
    this.saveLanguageToStorage(this.defaultSettings.language);
    this.saveUnitSystemToCookie(this.defaultSettings.unitSystem);
    this.settingsSignal.set(this.defaultSettings);
  }

  /**
   * Updates multiple settings at once
   */
  updateSettings(partial: Partial<UserSettings>): void {
    const current = this.getSettings();
    const updated = { ...current, ...partial };

    if (partial.language !== undefined) {
      this.saveLanguageToStorage(partial.language);
    }
    if (partial.unitSystem !== undefined) {
      this.saveUnitSystemToCookie(partial.unitSystem);
    }

    this.settingsSignal.set(updated);
  }

  /**
   * Deletes all settings
   */
  clearSettings(): void {
    localStorage.removeItem(this.LANGUAGE_STORAGE_KEY);
    this.deleteCookie(this.UNIT_SYSTEM_COOKIE_NAME);
    this.settingsSignal.set(this.defaultSettings);
  }

  /**
   * Returns a readonly signal of the settings
   */
  get settings$() {
    return this.settingsSignal.asReadonly();
  }
}
