import { ChangeDetectionStrategy, Component } from '@angular/core';

import { CdbCalculator } from './features/cdb-calculator/cdb-calculator';

@Component({
  selector: 'app-root',
  imports: [CdbCalculator],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {}
