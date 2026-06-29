import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CdbCalculationRequest, CdbCalculationResult } from '../models/cdb-calculation';
import { API_BASE_URL } from './api-base-url.token';

@Injectable({ providedIn: 'root' })
export class CdbCalculatorApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  calculate(request: CdbCalculationRequest): Observable<CdbCalculationResult> {
    return this.http.post<CdbCalculationResult>(`${this.baseUrl}/cdb/calculations`, request);
  }
}
