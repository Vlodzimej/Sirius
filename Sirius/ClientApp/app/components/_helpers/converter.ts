import { Batch } from '../_models';
import { ISelectOption } from '../_extends';

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
    public static ConvertToSelectOptionArray(objectArray: any[]) : ISelectOption[] {
        var obj = objectArray as IMetaObject[];
        var optionEntries: ISelectOption[] = [];
        obj.forEach(x => {
            var optionEntry: ISelectOption = { text: x.name, id: x.id };
            optionEntries.push(optionEntry);
        });
        return optionEntries;
    }


    public static BatchToSelectOptionArray(objectArray: any[]) : ISelectOption[] {
        var obj = objectArray as Batch[];
        var optionEntries: ISelectOption[] = [];
        obj.forEach(x => {
            var label = x.amount + ' шт. по цене ' + x.cost + ' руб.';
            var optionEntry: ISelectOption = { text: label, id: x.cost.toString() };
            optionEntries.push(optionEntry);
        });
        return optionEntries;
    }
}

export interface IMetaObject {
    id: string;
    name: string;
}