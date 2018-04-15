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