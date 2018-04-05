import { Vendor, User, Register } from ".";

export class Invoice {
    id: string;
    name: string;
    userId: string;
    userFullName: string;
    vendorId: string;
    vendorName: string;
    registers: Register[];
    createDate: string;
    isFixed: boolean;
    isTemporary: boolean;
}

export class InvoiceListItem {
    id: string;
    name: string;
    userFullName: string;
    createDate: string;
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
    createDate: string;
}