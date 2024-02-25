'use client';

import { useCallback, useMemo, useState } from 'react';
import { TrashIcon } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog';
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from '@/components/ui/drawer';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { useDeletePlannedTransaction } from '@/data/planned-transaction/delete-planned-transaction';
import { useMediaQuery } from '@/hooks/use-media-query';
import {
  ActionType,
  MonthlyPlannedTransactionDto,
  MonthlyRequest,
} from '@/types';

export function DeleteButton({
  plannedTransaction,
  monthlyRequest,
}: {
  plannedTransaction: MonthlyPlannedTransactionDto;
  monthlyRequest: MonthlyRequest;
}) {
  const t = useTranslations(
    'Pages.Dashboard.PlanedTransactionsList.DeleteButton'
  );
  const [open, setOpen] = useState(false);
  const isSmallDevice = useMediaQuery('only screen and (max-width : 768px)');

  const deleteMutation = useDeletePlannedTransaction();

  const handleButtonClick = useCallback(
    (actionType: ActionType) => {
      deleteMutation.mutate({
        plannedTransactionId: plannedTransaction.id,
        monthlyRequest: monthlyRequest,
        actionType: actionType,
      });
      setOpen(false);
    },
    [deleteMutation, plannedTransaction.id, monthlyRequest]
  );

  const content = useMemo(
    () => (
      <div className='flex flex-col space-y-2'>
        <Button
          aria-label={t('All')}
          onClick={() => handleButtonClick(ActionType.All)}
        >
          {t('All')}
        </Button>
        <Button
          aria-label={t('JustOne')}
          onClick={() => handleButtonClick(ActionType.JustOne)}
        >
          {t('JustOne')}
        </Button>
        <Button
          aria-label={t('FromNowOn')}
          onClick={() => handleButtonClick(ActionType.FromNowOn)}
        >
          {t('FromNowOn')}
        </Button>
      </div>
    ),
    [handleButtonClick, t]
  );

  const trigger = useMemo(
    () => (
      <TooltipTrigger className='flex flex-row' asChild>
        <Button variant='ghost' size='xs' aria-label={t('Title')}>
          <TrashIcon className='h-4 w-4' />
        </Button>
      </TooltipTrigger>
    ),
    [t]
  );

  const modal = useMemo(() => {
    if (isSmallDevice) {
      return (
        <Drawer open={open} onOpenChange={setOpen}>
          <DrawerTrigger asChild>{trigger}</DrawerTrigger>
          <DrawerContent>
            <DrawerHeader className='text-left'>
              <DrawerTitle>{t('Title')}</DrawerTitle>
              <DrawerDescription>{t('Description')}</DrawerDescription>
            </DrawerHeader>
            <div className='mb-4 w-[90%] justify-center self-center'>
              {content}
            </div>
            <DrawerFooter className='pt-2'>
              <Button variant='default' asChild></Button>
              <DrawerClose asChild>
                <Button variant='outline'>{t('Cancel')}</Button>
              </DrawerClose>
            </DrawerFooter>
          </DrawerContent>
        </Drawer>
      );
    }

    return (
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogTrigger asChild>{trigger}</DialogTrigger>
        <DialogContent className='sm:max-w-[425px]'>
          <DialogHeader>
            <DialogTitle>{t('Title')}</DialogTitle>
            <DialogDescription>{t('Description')}</DialogDescription>
          </DialogHeader>
          {content}
        </DialogContent>
      </Dialog>
    );
  }, [isSmallDevice, open, t, trigger, content]);

  return (
    <Tooltip>
      {modal}
      <TooltipContent side='bottom'>
        <p>{t('Tooltip')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
