import { Pipe, PipeTransform } from '@angular/core'
import { CurrencyPipe } from '@angular/common';

@Pipe({name: 'localizedCurrency'})
export class LocalizedCurrencyPipe implements PipeTransform {
  transform(value: any, symbolDisplay: boolean = false, digits: any = null): any {
    return new CurrencyPipe('ru-RU').transform(value, 'RUB', true, digits);

  }
}