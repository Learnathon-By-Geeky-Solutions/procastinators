export type UserInfo = {
    id: string;
    email: string;
    userName: string;
};

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

export type TotalPerCategory = {
    categoryId: string;
    categoryTitle: string;
    total: number;
};
export type TransactionReport = {
    categories: TotalPerCategory[];
    grandTotal: number;
};

export type Loan = {
    id: 17;
    lender?: UserInfo;
    borrower?: UserInfo;
    amount: number;
    note?: string;
    issuedAt: string;
    dueDate: string;
    dueAmount: number;
};
