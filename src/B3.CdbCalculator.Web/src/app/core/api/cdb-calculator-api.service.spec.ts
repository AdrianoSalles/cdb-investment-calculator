import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { API_BASE_URL } from './api-base-url.token';
import { CdbCalculatorApiService } from './cdb-calculator-api.service';

describe('CdbCalculatorApiService', () => {
  let service: CdbCalculatorApiService;
  let httpController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: API_BASE_URL, useValue: '/test-api' },
      ],
    });

    service = TestBed.inject(CdbCalculatorApiService);
    httpController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpController.verify());

  it('should send the calculation request to the API', () => {
    const request = { initialAmount: 1000, months: 12 };
    const response = {
      initialAmount: 1000,
      months: 12,
      grossAmount: 1123.08,
      grossEarnings: 123.08,
      taxRate: 0.2,
      taxAmount: 24.62,
      netAmount: 1098.46,
    };

    service.calculate(request).subscribe((result) => expect(result).toEqual(response));

    const httpRequest = httpController.expectOne('/test-api/cdb/calculations');
    expect(httpRequest.request.method).toBe('POST');
    expect(httpRequest.request.body).toEqual(request);
    httpRequest.flush(response);
  });
});
