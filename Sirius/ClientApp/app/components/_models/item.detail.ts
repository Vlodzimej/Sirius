import { Dimension, Category, Vendor } from ".";

export class ItemDetail
{
    id: string;
    name: string;
    dimension: Dimension = new Dimension();
    category: Category = new Category();
    vendor: Vendor = new Vendor();
}