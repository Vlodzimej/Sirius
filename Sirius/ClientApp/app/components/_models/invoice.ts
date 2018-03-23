import { Vendor, User, Register } from ".";

export class Invoice 
{
    id: string;
    name: string;
    vendor: Vendor;
    user: User;
    registers: Register[];
    isRecorded: boolean;
    createDate: string;
}

export class InvoiceListItem
{
    id: string;
    name: string;
    author: string;
    createDate: string;
}