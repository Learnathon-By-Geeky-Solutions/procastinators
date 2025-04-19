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

export type Transaction = {
    id: string;
    transactionType: string;
    amount: number;
    timestamp: string;
    note?: string;
    walletId: string;
    categoryId: string;
};
