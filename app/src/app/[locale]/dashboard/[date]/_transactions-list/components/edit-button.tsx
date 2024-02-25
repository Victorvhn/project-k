import { PencilIcon } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { Link } from '@/lib/navigation';
import { MonthlyRequest } from '@/types';

export function EditButton({
  transactionId,
  monthlyRequest,
}: {
  transactionId: string;
  monthlyRequest: MonthlyRequest;
}) {
  const t = useTranslations('Pages.Dashboard.TransactionsList.EditButton');

  return (
    <Tooltip>
      <TooltipTrigger className='flex flex-row' asChild>
        <Button
          variant='ghost'
          size='xs'
          aria-label='Delete transaction button'
          asChild
        >
          <Link
            href={{
              pathname: `/transactions/${transactionId}/edit`,
              query: {
                year: monthlyRequest.year,
                month: monthlyRequest.month,
              },
            }}
            className='size-4'
            aria-label='Go to create transaction page'
          >
            <PencilIcon />
          </Link>
        </Button>
      </TooltipTrigger>
      <TooltipContent side='bottom'>
        <p>{t('Tooltip')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
