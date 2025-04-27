export interface ApiTransaction {
  pk: string;
  transactionId: string;
  customerId: string;
  fundId: number;
  fundName: string;
  operationType: string;
  amount: number;
  timestamp: Date;
  notificationType: string;
}