import { MonthlyRequest } from '../monthly-request';

export type PayPlannedTransactionRequest = {
  plannedTransactionId: string;
  monthlyRequest: MonthlyRequest;
  amount: number;
};
