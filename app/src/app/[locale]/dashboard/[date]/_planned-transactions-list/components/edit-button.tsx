import { PencilIcon } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { ByDeviceModal } from '@/components/by-device-modal';
import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { Link } from '@/lib/navigation';
import {
  ActionType,
  MonthlyPlannedTransactionDto,
  MonthlyRequest,
} from '@/types';

export default function EditButton({
  plannedTransaction,
  monthlyRequest,
}: {
  plannedTransaction: MonthlyPlannedTransactionDto;
  monthlyRequest: MonthlyRequest;
}) {
  const t = useTranslations(
    'Pages.Dashboard.PlanedTransactionsList.EditButton'
  );

  return (
    <Tooltip>
      <ByDeviceModal
        title={t('Title')}
        description={t('Description')}
        trigger={
          <TooltipTrigger className='flex flex-row' asChild>
            <Button
              variant='ghost'
              size='xs'
              aria-label={t('EditPlannedTransactionButtonDescription')}
            >
              <PencilIcon className='h-4 w-4' />
            </Button>
          </TooltipTrigger>
        }
      >
        <div className='flex flex-col space-y-2'>
          <Button aria-label='Edit all button' asChild>
            <Link
              href={{
                pathname: `/planned-transactions/${plannedTransaction.id}/edit`,
                query: {
                  year: monthlyRequest.year,
                  month: monthlyRequest.month,
                  actionType: ActionType.All,
                },
              }}
              className='h-10 px-4 py-2'
              aria-label={t('EditAllPlannedTransactionLinkDescription')}
            >
              {t('All')}
            </Link>
          </Button>
          <Button aria-label='Edit just one button' asChild>
            <Link
              href={{
                pathname: `/planned-transactions/${plannedTransaction.id}/edit`,
                query: {
                  year: monthlyRequest.year,
                  month: monthlyRequest.month,
                  actionType: ActionType.JustOne,
                },
              }}
              className='h-10 px-4 py-2'
              aria-label={t('EditJustOnePlannedTransactionLinkDescription')}
            >
              {t('JustOne')}
            </Link>
          </Button>
          <Button aria-label='Edit from now on button' asChild>
            <Link
              href={{
                pathname: `/planned-transactions/${plannedTransaction.id}/edit`,
                query: {
                  year: monthlyRequest.year,
                  month: monthlyRequest.month,
                  actionType: ActionType.FromNowOn,
                },
              }}
              className='h-10 px-4 py-2'
              aria-label={t('EditFromNowOnPlannedTransactionLinkDescription')}
            >
              {t('FromNowOn')}
            </Link>
          </Button>
        </div>
      </ByDeviceModal>
      <TooltipContent side='bottom'>
        <p>{t('Tooltip')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
