import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';

import { CdbCalculatorApiService } from '../../core/api/cdb-calculator-api.service';
import { CdbCalculator } from './cdb-calculator';

registerLocaleData(localePt);

describe('CdbCalculator', () => {
  let fixture: ComponentFixture<CdbCalculator>;
  const api = {
    calculate: vi.fn(),
  };

  beforeEach(async () => {
    api.calculate.mockReset();

    await TestBed.configureTestingModule({
      imports: [CdbCalculator],
      providers: [{ provide: CdbCalculatorApiService, useValue: api }],
    }).compileComponents();

    fixture = TestBed.createComponent(CdbCalculator);
    fixture.detectChanges();
  });

  it('should not call the API when the form is invalid', () => {
    fixture.componentInstance.form.setValue({ initialAmount: 0, months: 1 });

    fixture.componentInstance.submit();

    expect(api.calculate).not.toHaveBeenCalled();
  });

  it('should render the result returned by the API', () => {
    api.calculate.mockReturnValue(
      of({
        initialAmount: 1000,
        months: 12,
        grossAmount: 1123.08,
        grossEarnings: 123.08,
        taxRate: 0.2,
        taxAmount: 24.62,
        netAmount: 1098.46,
      }),
    );

    fixture.componentInstance.submit();
    fixture.detectChanges();

    const text = fixture.debugElement.query(By.css('.calculator-card__results')).nativeElement
      .textContent as string;

    expect(api.calculate).toHaveBeenCalledWith({ initialAmount: 1000, months: 12 });
    expect(text).toContain('1.123,08');
    expect(text).toContain('1.098,46');
    expect(text).toContain('20%');
  });
});
