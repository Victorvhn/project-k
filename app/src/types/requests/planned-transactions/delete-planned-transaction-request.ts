import { ActionType, MonthlyRequest } from '@/types';

export type DeletePlannedTransactionRequest = {
  plannedTransactionId: string;
  actionType: ActionType;
  monthlyRequest: MonthlyRequest;
};
