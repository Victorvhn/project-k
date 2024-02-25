import { TransactionType } from '@/types/enums/transaction-type';

import { MonthlyRequest } from '../monthly-request';

export type GetSummaryRequest = {
  monthlyRequest: MonthlyRequest;
  transactionType: TransactionType;
};
