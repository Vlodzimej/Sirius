import { Vendor, User, Register } from ".";

export class Invoice 
{
    id: string;
    name: string;
    userId: string;
    userFullName: string;
    vendorId: string;
    vendorName: string;
    registers: Register[];
    createDate: string;
    isRecorded: boolean;
    isTemporary: boolean;
}

export class InvoiceListItem
{
    id: string;
    name: string;
    userFullName: string;
    createDate: string;
}