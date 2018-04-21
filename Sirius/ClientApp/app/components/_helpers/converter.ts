import { IOption } from 'ng-select';

export class Converter {
    /**
     * OptionsConverter
     * Конвертирование массива объектов в массив объектов для выпадающего списка select2
     */
    public static ConvertToOptionArray(objectArray: any[]) : IOption[] {
        var obj = objectArray as MetaObject[];
        var optionEntries: IOption[] = [];
        obj.forEach(x => {
            var optionEntry: IOption = { label: x.name, value: x.id };
            optionEntries.push(optionEntry);
        });
        return optionEntries;
    }
}

export class MetaObject {
    id: string;
    name: string;
}