import { Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

@Component({ template: '' })
export class DestroyableComponent implements OnDestroy {
  public onDestroy$ = new Subject();

  ngOnDestroy(): void {
    this.onDestroy$.next(void 0);
  }
}
