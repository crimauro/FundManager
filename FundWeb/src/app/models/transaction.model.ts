export type NotificationType = 'Email' | 'SMS';
export type OperationType = 'Apertura' | 'Cancelación';

export interface Transaction {
    id: number;
    fundId: number;
    fundName: string;
    operationType: OperationType;
    amount: number;
    dateTime: Date;
    notificationType: NotificationType;
}
