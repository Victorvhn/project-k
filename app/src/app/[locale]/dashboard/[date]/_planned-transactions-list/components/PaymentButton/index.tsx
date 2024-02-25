'use client';

import { MonthlyPlannedTransactionDto, MonthlyRequest } from '@/types';

import CancelPaymentButton from './cancel-payment-button';
import PayModal from './pay-modal';

export function PaymentButton({
  plannedTransaction,
  monthlyRequest,
}: {
  plannedTransaction: MonthlyPlannedTransactionDto;
  monthlyRequest: MonthlyRequest;
}) {
  return plannedTransaction.paid === true ? (
    <CancelPaymentButton
      plannedTransaction={plannedTransaction}
      monthlyRequest={monthlyRequest}
    />
  ) : (
    <PayModal
      plannedTransaction={plannedTransaction}
      monthlyRequest={monthlyRequest}
    />
  );
}
