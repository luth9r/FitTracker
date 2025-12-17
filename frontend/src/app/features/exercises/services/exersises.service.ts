import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ExerciseResponse {
  name: string;
  description?: string | null;
  imageUrl?: string | null;
  videoUrl?: string | null;
  muscleGroup: string;
  equipment: string;
  isCustom: boolean;
}

export type ExerciseFilterType = 'All' | 'Standard' | 'Custom';

@Injectable({ providedIn: 'root' })
export class ExerciseService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5000/api/exercise';

  getExercises(type: ExerciseFilterType = 'All'): Observable<ExerciseResponse[]> {
    return this.http.get<ExerciseResponse[]>(this.baseUrl, {
      params: { type },
      withCredentials: true,
    });
  }
}
