export class FilterParameters {
    /** Название */
    name?: boolean = false;
    /** Категория */
    category?: boolean = false;
    /** Наименование */
    item?: boolean = false;
    /** Поставщик */
    vendor?: boolean = false;
    /** Выбор интервала дат */
    date?: boolean = false;
    /** Показать только проведённые накладные */
    fixedOnly?: boolean = false;
    /** Считать остатки динамически */
    isDynamic?: boolean = false;
    /** Показывать только критические остатки */
    isMinimumLimit?: boolean = false;
}