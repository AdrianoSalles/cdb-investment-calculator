export interface CdbCalculationRequest {
  initialAmount: number;
  months: number;
}

export interface CdbCalculationResult {
  initialAmount: number;
  months: number;
  grossAmount: number;
  grossEarnings: number;
  taxRate: number;
  taxAmount: number;
  netAmount: number;
}
