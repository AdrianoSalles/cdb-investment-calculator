import { ChangeDetectionStrategy, Component, input } from '@angular/core';

export type IconName =
  'calculator' | 'chart' | 'coins' | 'wallet' | 'receipt' | 'percent' | 'info' | 'arrow-right';

@Component({
  selector: 'app-icon',
  template: `
    <svg aria-hidden="true" focusable="false" [style.width.px]="size()" [style.height.px]="size()">
      <use [attr.href]="'icons.svg#' + name()"></use>
    </svg>
  `,
  styles: `
    :host {
      display: inline-flex;
      flex: 0 0 auto;
      line-height: 0;
    }

    svg {
      display: block;
      fill: none;
      stroke: currentColor;
      stroke-linecap: round;
      stroke-linejoin: round;
      stroke-width: 2;
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Icon {
  readonly name = input.required<IconName>();
  readonly size = input(20);
}
