import { Dimension, Category } from ".";

export class Item 
{
    id: string;
    name: string;
    dimensionId: string;
    categoryId: string;
    dimension: Dimension = new Dimension();
    category: Category = new Category();
}
