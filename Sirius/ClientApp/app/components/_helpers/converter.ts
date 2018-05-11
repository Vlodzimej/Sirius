import { Batch } from '../_models';

export class Converter {
    /**
     * OptionsConverter
     * Конвертирование массива объектов в массив объектов для выпадающего списка select2
     *//*
    public static ConvertToOptionArray(objectArray: any[]) : IOption[] {
        var obj = objectArray as MetaObject[];
        var optionEntries: IOption[] = [];
        obj.forEach(x => {
            var optionEntry: IOption = { label: x.name, value: x.id };
            optionEntries.push(optionEntry);
        });
        return optionEntries;
    }

    public static BatchToOptionArray(objectArray: any[]) : IOption[] {
        var obj = objectArray as Batch[];
        var optionEntries: IOption[] = [];
        obj.forEach(x => {
            var label = x.amount + ' шт. по цене ' + x.cost + ' руб.';
            var optionEntry: IOption = { label: label, value: x.cost.toString() };
            optionEntries.push(optionEntry);
        });
        return optionEntries;
    } */
}

export class MetaObject {
    id: string;
    name: string;
}