import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'currency'})
export class CurrencyPipe implements PipeTransform {
    transform(value: string): string {
        // Определению валюты
        var currency = " руб.";
        // Изменение строки
        return value + currency;
    }
}