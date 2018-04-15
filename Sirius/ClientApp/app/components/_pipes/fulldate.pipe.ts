import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'fulldate',
    pure: false
})
export class FullDatePipe implements PipeTransform {
    transform(value: string): string {
        var result : string = value;
        var elements: string[] = value.split('.');

        if (elements.length == 3) {
            // Переводим в number, тем самым избавляемся от нуля 
            var day = parseInt(elements[0]);
            var month: string = '';
            var year: string = elements[2];

            switch (elements[1]) {
                case '01': month = 'января'; break;
                case '02': month = 'февраля'; break;
                case '03': month = 'марта'; break;
                case '04': month = 'апреля'; break;
                case '05': month = 'мая'; break;
                case '06': month = 'июня'; break;
                case '07': month = 'июля'; break;
                case '08': month = 'августа'; break;
                case '09': month = 'сентября'; break;
                case '10': month = 'октября'; break;
                case '11': month = 'ноября'; break;
                case '12': month = 'декабря'; break;
            }
            result = day + ' ' + month + ' ' + year + ' г.';
        }
        return result;
    }
}