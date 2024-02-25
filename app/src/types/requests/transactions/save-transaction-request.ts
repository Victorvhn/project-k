import { TransactionType } from '@/types';

interface Base {
  description: string;
  amount: number;
  type: TransactionType;
  paidAt: string;
  categoryId?: string;
  plannedTransactionId: string | null;
}

export interface CreateTransactionRequest extends Base {}

export interface EditTransactionRequest extends Base {}
