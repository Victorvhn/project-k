import { PlannedTransactionDto, TransactionType } from '..';

export type MonthlyTransactionDto = {
  id: string;
  description: string;
  amount: number;
  type: TransactionType;
  paidAt: string;
  categoryId?: string;
  plannedTransactionId?: string;
  plannedTransaction?: PlannedTransactionDto;

  // For optimistic updates
  isChanging?: boolean;
};
