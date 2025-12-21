import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ExerciseResponse {
  id: string;
  name: string;
  description?: string | null;
  imageUrl?: string | null;
  videoUrl?: string | null;
  muscleGroup: string;
  equipment: string;
  isCustom: boolean;
}

export interface ExerciseDetailsResponse extends ExerciseResponse {
  maxWeightKg: number;
  maxReps: number;
  maxVolume: number;
  maxTotalVolume: number;
  maxWeightDate?: string | null;
  maxRepsDate?: string | null;
  maxVolumeDate?: string | null;
  maxTotalVolumeDate?: string | null;

  totalWorkouts: number;
  totalSets: number;
  totalReps: number;
  totalLifted: number;
  avgWeightPerSet: number;
  avgRepsPerSet: number;
  lastPerformed?: string | null;
  volumeHistory: ExerciseHistoryPointResponse[];
}

export interface ExerciseHistoryPointResponse {
  date: string;
  value: number;
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

  getExerciseDetails(id: string): Observable<ExerciseDetailsResponse> {
    return this.http.get<ExerciseDetailsResponse>(
      `${this.baseUrl}/${id}`,
      { withCredentials: true }
    );
  }
}
