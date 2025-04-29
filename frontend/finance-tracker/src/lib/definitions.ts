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
    id: string;
    lender?: UserInfo;
    borrower?: UserInfo;
    amount: number;
    note?: string;
    issuedAt: string;
    dueDate: string;
    dueAmount: number;
};

export type LoanRequest = {
    id: string;
    amount: number;
    note?: string;
    dueDate: string;
    borrower: UserInfo;
    lender: UserInfo;
    isApproved: boolean;
};

export type LoanClaim = {
    id: number;
    loan: Loan;
    isClaimed: boolean;
    claimedAt: string;
};

export type Installment = {
    id: number;
    loan: Loan;
    timestamp: string;
    amount: number;
    note?: string;
    nextDueDate: string;
};

export type InstallmentClaim = {
    id: number;
    installment: Installment;
    isClaimed: boolean;
    claimedAt: string;
};
