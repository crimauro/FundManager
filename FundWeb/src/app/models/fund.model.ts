export interface Fund {
    id: number;
    name: string;
    minimumSubscriptionAmount: number;
}

export interface UserFund extends Fund {
    subscribedAmount: number;
}
