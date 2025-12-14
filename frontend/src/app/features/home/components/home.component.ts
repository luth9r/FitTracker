import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';
import { forkJoin } from 'rxjs';
import { HomeService, UserStatsResponse, RecentWorkoutResponse } from '../services/home.service';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule, TranslateModule, MatIconModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  private homeService = inject(HomeService);

  stats: UserStatsResponse | null = null;
  recentWorkouts: RecentWorkoutResponse[] = [];
  isLoading = true;
  error: string | null = null;

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    this.isLoading = true;
    this.error = null;

    forkJoin({
      stats: this.homeService.getStats(),
      workouts: this.homeService.getRecentWorkouts(5),
    }).subscribe({
      next: ({ stats, workouts }) => {
        this.stats = stats;
        this.recentWorkouts = workouts;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load data';
        this.isLoading = false;
      },
    });
  }

  /**
   * Formats ISO date string to localized date string.
   * @param dateIso ISO date string
   * @returns Localized date string
   */
  formatDate(dateIso: string): string {
    const d = new Date(dateIso);
    return d.toLocaleDateString();
  }

  /**
   * Converts weight from kg to display units based on user preferences.
   * @param weightInKg Weight in kilograms
   * @returns Weight in current unit system (kg or lbs)
   */
  convertWeight(weightInKg: number): number {
    return this.homeService.convertWeight(weightInKg);
  }

  /**
   * Gets the current unit label.
   * @returns 'kg' or 'lbs'
   */
  get unitLabel(): string {
    return this.homeService.getUnitLabel();
  }

  /**
   * Gets total weight lifted in current units.
   * @returns Total weight lifted
   */
  get totalWeightLifted(): number {
    return this.stats ? this.convertWeight(this.stats.totalWeightLiftedKg) : 0;
  }

  /**
   * Calculates average volume per workout in current units.
   * @returns Average volume per workout
   */
  get avgVolumePerWorkout(): number {
    if (!this.stats || this.stats.totalWorkouts === 0) {
      return 0;
    }
    const avgInKg = this.stats.totalWeightLiftedKg / this.stats.totalWorkouts;
    return this.convertWeight(avgInKg);
  }

  /**
   * Calculates progress percentage for average volume goal.
   * @returns Progress percentage (0-100)
   */
  get avgVolumeProgressPercent(): number {
    const targetKg = 10000;
    const target = this.convertWeight(targetKg);

    if (this.avgVolumePerWorkout <= 0) {
      return 0;
    }
    const percent = (this.avgVolumePerWorkout / target) * 100;
    return percent > 100 ? 100 : percent;
  }
}
