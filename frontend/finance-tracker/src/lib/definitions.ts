export type Wallet = {
    id: string;
    name: string;
    type: string;
    currency: string;
    balance: number;
};

export type Category = {
    id: string;
    title: string;
    defaultTransactionType: string;
};
