import { Dimension, Category, Vendor } from ".";

export class ItemDetail
{
    id: string;
    name: string;
    dimensionId: string;
    categoryId: string;
    dimension: Dimension = new Dimension();
    category: Category = new Category();
    isCountless: boolean;
}