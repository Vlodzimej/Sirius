import { Item, Invoice } from ".";

export class Register 
{
    id: string;
    cost: number;
    amount: number;
    item: Item = new Item();
    invoice: Invoice = new Invoice();
}