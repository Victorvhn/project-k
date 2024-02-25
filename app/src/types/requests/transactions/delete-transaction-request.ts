import { MonthlyRequest } from '../monthly-request';

export type DeleteTransactionRequest = {
  transactionId: string;
  monthlyRequest: MonthlyRequest;
};
