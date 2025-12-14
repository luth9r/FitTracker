import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserStatsResponse {
  totalWorkouts: number;
  trainingDays: number;
  longestStreak: number;
  totalWeightLiftedKg: number;
}

export interface RecentWorkoutResponse {
  id: string;
  workoutDate: string;
  name: string;
  isCompleted: boolean;
  durationMinutes: number;
  totalVolumeKg: number;
}

export type UnitSystem = 'metric' | 'imperial';

@Injectable({ providedIn: 'root' })
export class HomeService {
  private http = inject(HttpClient);
  private userApiUrl = 'http://localhost:5000/api/user';

  constructor() {
    // Initialize preferred-units cookie on service load
    this.ensurePreferredUnitsCookie();
  }

  getStats(): Observable<UserStatsResponse> {
    return this.http.get<UserStatsResponse>(`${this.userApiUrl}/stats`, {
      withCredentials: true
    });
  }

  getRecentWorkouts(take = 5): Observable<RecentWorkoutResponse[]> {
    return this.http.get<RecentWorkoutResponse[]>(
      `${this.userApiUrl}/workouts/recent`,
      {
        params: { take },
        withCredentials: true
      }
    );
  }

  /**
   * Converts weight from kilograms to the current unit system.
   * @param valueInKg Weight in kilograms
   * @returns Weight in the current unit system
   */
  convertWeight(valueInKg: number): number {
    const unit = this.getPreferredUnitsFromCookie();
    if (unit === 'imperial') {
      return valueInKg * 2.20462; // kg -> lbs
    }
    return valueInKg;
  }

  /**
   * Returns the short unit label based on cookie value.
   * @returns 'kg' or 'lbs'
   */
  getUnitLabel(): string {
    const unit = this.getPreferredUnitsFromCookie();
    return unit === 'imperial' ? 'lbs' : 'kg';
  }

  /**
   * Sets preferred units in cookie.
   * @param unit Unit system
   */
  setPreferredUnits(unit: UnitSystem): void {
    const expires = new Date();
    expires.setDate(expires.getDate() + 365); // 1 year
    document.cookie = `preferred-units=${unit}; expires=${expires.toUTCString()}; path=/; SameSite=Lax`;
  }

  /**
   * Ensures preferred-units cookie exists, sets 'metric' if missing.
   */
  private ensurePreferredUnitsCookie(): void {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; preferred-units=`);
    if (parts.length < 2) {
      this.setPreferredUnits('metric');
    }
  }

  /**
   * Reads preferred units from cookie.
   * @returns UnitSystem from cookie or 'metric' by default
   */
  private getPreferredUnitsFromCookie(): UnitSystem {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; preferred-units=`);
    if (parts.length === 2) {
      const unit = parts.pop()?.split(';').shift();
      return (unit === 'imperial' ? 'imperial' : 'metric') as UnitSystem;
    }
    return 'metric';
  }
}
