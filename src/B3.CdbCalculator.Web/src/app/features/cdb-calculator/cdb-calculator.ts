import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  ElementRef,
  ViewChild,
  computed,
  inject,
  signal,
} from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';

import { CdbCalculatorApiService } from '../../core/api/cdb-calculator-api.service';
import { CdbCalculationResult } from '../../core/models/cdb-calculation';
import { Icon } from '../../shared/icon/icon';

@Component({
  selector: 'app-cdb-calculator',
  imports: [ReactiveFormsModule, CurrencyPipe, DecimalPipe, Icon],
  templateUrl: './cdb-calculator.html',
  styleUrl: './cdb-calculator.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CdbCalculator {
  private readonly api = inject(CdbCalculatorApiService);
  private readonly destroyRef = inject(DestroyRef);

  @ViewChild('resultsPanel')
  private readonly resultsPanel?: ElementRef<HTMLElement>;

  readonly form = new FormGroup({
    initialAmount: new FormControl(1000, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(0.01)],
    }),
    months: new FormControl(12, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(2), Validators.pattern(/^\d+$/)],
    }),
  });

  readonly result = signal<CdbCalculationResult | null>(null);
  readonly isLoading = signal(false);
  readonly hasSubmitted = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly taxRatePercentage = computed(() => {
    const currentResult = this.result();
    return currentResult ? currentResult.taxRate * 100 : null;
  });

  submit(): void {
    this.hasSubmitted.set(true);
    this.errorMessage.set(null);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    this.api
      .calculate(this.form.getRawValue())
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.isLoading.set(false)),
      )
      .subscribe({
        next: (calculation) => {
          this.result.set(calculation);
          queueMicrotask(() => this.resultsPanel?.nativeElement.focus());
        },
        error: (error) => this.errorMessage.set(this.getErrorMessage(error)),
      });
  }

  showInitialAmountError(): boolean {
    const control = this.form.controls.initialAmount;
    return control.invalid && (control.touched || this.hasSubmitted());
  }

  showMonthsError(): boolean {
    const control = this.form.controls.months;
    return control.invalid && (control.touched || this.hasSubmitted());
  }

  private getErrorMessage(error: unknown): string {
    if (!(error instanceof HttpErrorResponse)) {
      return 'Não foi possível concluir a simulação. Tente novamente.';
    }

    if (error.status === 0) {
      return 'Não foi possível conectar à API. Confirme se o backend está em execução.';
    }

    const validationErrors = error.error?.errors;

    if (validationErrors && typeof validationErrors === 'object') {
      const messages = Object.values(validationErrors)
        .flatMap((value) => (Array.isArray(value) ? value : []))
        .filter((value): value is string => typeof value === 'string');

      if (messages.length > 0) {
        return messages.join(' ');
      }
    }

    return 'A API não conseguiu calcular o investimento. Revise os dados e tente novamente.';
  }
}
