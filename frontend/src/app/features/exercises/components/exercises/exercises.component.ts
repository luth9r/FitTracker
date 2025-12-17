import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';
import {
  ExerciseResponse,
  ExerciseService,
  ExerciseFilterType,
} from '../../services/exersises.service';
import { BottomNavComponent } from '../../../../shared/ui/bottom-nav/bottom-nav.component';
import { FabComponent } from '../../../../shared/ui/fab/fab.component';
import { PageHeaderComponent } from '../../../../shared/ui/header/header.component';
import { InputFieldComponent } from '../../../../shared/ui/input-field/input-field.component';

type ExerciseWithState = ExerciseResponse & { imageError?: boolean };
type ExerciseTypeFilter = 'all' | 'standard' | 'custom';

interface FilterOption<T = string> {
  value: T;
  labelKey?: string;
}

@Component({
  standalone: true,
  selector: 'app-exercises',
  imports: [
    CommonModule,
    FormsModule,
    MatIconModule,
    BottomNavComponent,
    FabComponent,
    PageHeaderComponent,
    InputFieldComponent,
    TranslateModule,
  ],
  templateUrl: './exercises.component.html',
  styleUrls: ['./exercises.component.scss'],
})
export class ExercisesComponent implements OnInit {
  searchQuery = '';

  selectedCategory = 'all';
  selectedMuscle = 'all';
  selectedType: ExerciseTypeFilter = 'all';
  showFilters = false;

  @ViewChild('filtersSheet', { static: false })
  filtersSheetRef?: ElementRef<HTMLElement>;

  categoryOptions: FilterOption<string>[] = [];
  muscleOptions: FilterOption<string>[] = [];
  typeOptions: FilterOption<ExerciseTypeFilter>[] = [
    { value: 'all',      labelKey: 'EXERCISES.FILTERS.TYPE_ALL' },
    { value: 'standard', labelKey: 'EXERCISES.FILTERS.TYPE_STANDARD' },
    { value: 'custom',   labelKey: 'EXERCISES.FILTERS.TYPE_CUSTOM' },
  ];

  exercises: ExerciseWithState[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(private readonly exerciseService: ExerciseService) {}

  ngOnInit(): void {
    this.loadExercises();
  }

  loadExercises(type: ExerciseFilterType = 'All'): void {
    this.isLoading = true;
    this.error = null;

    this.exerciseService.getExercises(type).subscribe({
      next: (data) => {
        this.exercises = (data ?? []).map((ex) => ({
          ...ex,
          imageError: false,
        }));

        const equipmentSet = new Set<string>();
        const muscleSet = new Set<string>();

        for (const ex of this.exercises) {
          if (ex.equipment) {
            equipmentSet.add(ex.equipment);
          }
          if (ex.muscleGroup) {
            muscleSet.add(ex.muscleGroup);
          }
        }

        const equipments = Array.from(equipmentSet).sort();
        const muscles = Array.from(muscleSet).sort();

        this.categoryOptions = [
          { value: 'all', labelKey: 'EXERCISES.FILTERS.CATEGORY.ALL' },
          ...equipments.map((eq) => ({
            value: eq,
            // labelKey: `EXERCISES.EQUIPMENT.${eq.toUpperCase()}`
          })),
        ];

        this.muscleOptions = [
          { value: 'all', labelKey: 'EXERCISES.FILTERS.MUSCLE.ALL' },
          ...muscles.map((m) => ({
            value: m,
            // labelKey: `EXERCISES.MUSCLES.${m.toUpperCase()}`
          })),
        ];

        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load exercises';
        this.isLoading = false;
      },
    });
  }

  get filteredExercises(): ExerciseWithState[] {
    return this.exercises.filter((ex) => {
      const q = this.searchQuery.toLowerCase().trim();

      const matchesSearch =
        !q ||
        ex.name.toLowerCase().includes(q) ||
        ex.muscleGroup.toLowerCase().includes(q) ||
        ex.equipment.toLowerCase().includes(q);

      const category = ex.equipment;
      const matchesCategory =
        this.selectedCategory === 'all' || category === this.selectedCategory;

      const matchesMuscle =
        this.selectedMuscle === 'all' || ex.muscleGroup === this.selectedMuscle;

      const matchesType =
        this.selectedType === 'all' ||
        (this.selectedType === 'custom' && ex.isCustom) ||
        (this.selectedType === 'standard' && !ex.isCustom);

      return matchesSearch && matchesCategory && matchesMuscle && matchesType;
    });
  }

  get activeFiltersCount(): number {
    return (
      (this.selectedCategory !== 'all' ? 1 : 0) +
      (this.selectedMuscle !== 'all' ? 1 : 0) +
      (this.selectedType !== 'all' ? 1 : 0)
    );
  }

  clearFilters(): void {
    this.selectedCategory = 'all';
    this.selectedMuscle = 'all';
    this.selectedType = 'all';
  }

  openFilters(): void {
    this.showFilters = true;
  }

  closeFilters(): void {
    const sheet = this.filtersSheetRef?.nativeElement;
    if (!sheet) {
      this.showFilters = false;
      return;
    }

    sheet.classList.add('ex-filters-sheet--closing');

    const handleAnimationEnd = () => {
      sheet.removeEventListener('animationend', handleAnimationEnd);
      sheet.classList.remove('ex-filters-sheet--closing');
      this.showFilters = false;
    };

    sheet.addEventListener('animationend', handleAnimationEnd);
  }

  onImageLoadError(ex: ExerciseWithState): void {
    ex.imageError = true;
  }

  onFabClick(): void {
    // TODO: open create-exercise flow
  }
}
