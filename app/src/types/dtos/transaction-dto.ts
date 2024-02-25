import { TransactionType } from '..';

export type TransactionDto = {
  id: string;
  description: string;
  amount: number;
  type: TransactionType;
  paidAt: string;
  categoryId?: string;
  plannedTransactionId?: string;
};
