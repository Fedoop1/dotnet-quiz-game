import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'orderBy' })
export class OrderByPipe implements PipeTransform {
  transform(
    source: any[],
    comparator: (lhs: any, rhs: any) => number,
    order: 'asc' | 'desc' = 'asc'
  ) {
    const result = source.sort(comparator);
    return order === 'asc' ? result : result.reverse();
  }
}
