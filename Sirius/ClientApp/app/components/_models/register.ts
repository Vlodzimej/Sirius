import { Item, Invoice, Dimension } from ".";

export class Register 
{
    id: string;
    cost: number;
    amount: number;
    itemId: string;
    item: Item;
    invoiceId: string;
    invoice: Invoice;
}
