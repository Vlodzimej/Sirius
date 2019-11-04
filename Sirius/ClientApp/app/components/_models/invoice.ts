import { Vendor, User, Register } from ".";

export class Invoice {
    id: string;
    name: string;
    userId: string;
    userFullName: string;
    vendorId: string;
    vendorName: string;
    registers: Register[];
    date: string;
    isFixed: boolean;
    isTemporary: boolean;
    typeId: string;
    factor: number;
    comment: string;
}

export class InvoiceListItem {
    id: string;
    name: string;
    userFullName: string;
    date: string;
    isFixed: boolean;
    isTemporary: boolean;
}

export class InvoiceUpdate {
    id: string;
    name: string;
    vendorid: string;
    userid: string;
    isrecorderd: boolean;
    istemporary: boolean;
    date: string;
}

export class InvoiceType {
    id: string;
    name: string;
    factor: number;
    alias: string;
}