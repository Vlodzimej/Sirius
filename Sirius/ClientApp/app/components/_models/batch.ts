export class Batch 
{
    name: string;
    cost: number;
    amount: number;
    sum: number;
}

export class BatchGroup
{
    name: string;
    batches: Batch[];
}

export class BatchListElement 
{
    text: string;
    cost: number;
    amount: number;
    sum: number;
    type: BatchListElementType;
}
export enum BatchListElementType {
    Head, Batch, Footer
}