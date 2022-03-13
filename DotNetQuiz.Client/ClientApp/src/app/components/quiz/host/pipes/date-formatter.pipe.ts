import { Pipe, PipeTransform } from '@angular/core';
import { DateFormat } from '../models/date-format.enum';

@Pipe({ name: 'dateFormatter' })
export class DateFormatterPipe implements PipeTransform {
  transform(date: Date | number, dateFormat: DateFormat) {
    const result = date instanceof Date ? date : new Date(date);
    switch (dateFormat) {
      case DateFormat.LocaleString:
        return result.toLocaleString();
      case DateFormat.LocaleTimeString:
        return result.toLocaleTimeString();
      case DateFormat.LocaleDateString:
        return result.toLocaleDateString();
    }
  }
}
