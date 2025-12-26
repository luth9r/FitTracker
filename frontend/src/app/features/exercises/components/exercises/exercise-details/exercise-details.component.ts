import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { ExerciseDetailsResponse, ExerciseService } from '../../../services/exersises.service';
import { TranslateModule } from '@ngx-translate/core';
import { UtilsService } from '../../../../../shared/utils/utils.service';
import { EmptyStateComponent } from '../../../../../shared/ui/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../../../../shared/ui/error-state/error-state.component';

interface VolumeHistoryPoint {
  date: string;
  value: number;
}

type ChartPeriod = '1m' | '3m' | '6m' | '1y';

@Component({
  standalone: true,
  selector: 'app-exercise-details',
  imports: [CommonModule, MatIconModule, TranslateModule, EmptyStateComponent, ErrorStateComponent],
  templateUrl: './exercise-details.component.html',
  styleUrl: './exercise-details.component.scss',
})
export class ExerciseDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private exerciseService = inject(ExerciseService);
  private router = inject(Router);
  private utils = inject(UtilsService);

  exercise?: ExerciseDetailsResponse & { volumeHistory: VolumeHistoryPoint[] };
  isLoading = true;
  error: string | null = null;
  activeTab = 'info';
  imageError = false;
  chartPeriod: ChartPeriod = '3m';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = 'No id in route';
      this.isLoading = false;
      return;
    }

    this.exerciseService.getExerciseDetails(id).subscribe({
      next: (data) => {
        this.exercise = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load exercise';
        this.isLoading = false;
      },
    });
  }

  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  setChartPeriod(period: ChartPeriod): void {
    this.chartPeriod = period;
  }

  onClose(): void {
    this.router.navigate(['/exercises']);
  }

  onImageError(): void {
    this.imageError = true;
  }

  convertWeight(weightInKg: number): number {
    return this.utils.convertWeight(weightInKg);
  }

  get unitLabel(): string {
    return this.utils.getUnitLabel();
  }

  formatDate(dateString: string | null | undefined): string {
    return this.utils.formatDate(dateString);
  }

  getFormattedDate(dateString: string): string {
    return this.utils.formatDateShort(dateString);
  }

  getFilteredHistory(): VolumeHistoryPoint[] {
    if (!this.exercise?.volumeHistory) return [];

    const now = new Date();
    const periodMap: Record<ChartPeriod, number> = {
      '1m': 30,
      '3m': 90,
      '6m': 180,
      '1y': 365,
    };

    const daysAgo = periodMap[this.chartPeriod];
    const cutoffDate = new Date(now.getTime() - daysAgo * 24 * 60 * 60 * 1000);

    return this.exercise.volumeHistory
      .filter((item) => {
        const itemDate = new Date(item.date);
        return itemDate >= cutoffDate;
      })
      .map((item) => ({
        ...item,
        value: this.convertWeight(item.value),
      }));
  }

  getBarHeight(item: VolumeHistoryPoint): number {
    const filteredData = this.getFilteredHistory();
    if (!filteredData.length) return 0;
    const maxVol = Math.max(...filteredData.map((v) => v.value));
    return maxVol > 0 ? (item.value / maxVol) * 100 : 0;
  }

  trackByIndex(index: number): number {
    return index;
  }

  retry(): void {
    this.error = null;
    this.isLoading = true;
    this.imageError = false;

    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = 'No id in route';
      this.isLoading = false;
      return;
    }

    this.exerciseService.getExerciseDetails(id).subscribe({
      next: (data) => {
        this.exercise = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load exercise';
        this.isLoading = false;
      },
    });
  }
}
