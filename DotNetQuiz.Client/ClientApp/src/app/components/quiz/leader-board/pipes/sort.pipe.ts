import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'sort' })
export class SortPipe implements PipeTransform {
  transform(source: any[], comparator: (lhs: any, rhs: any) => number) {
    return source.sort(comparator);
  }
}
