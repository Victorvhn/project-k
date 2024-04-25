'use client';

import { useState } from 'react';
import { Check, ChevronsUpDown, Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { Button } from '@/components/ui/button';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import { FormControl } from '@/components/ui/form';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { useFetchPlannedTransactionsPaginated } from '@/data/planned-transaction/fetch-planned-transactions';
import { cn } from '@/lib/utils';
import { PlannedTransactionDto } from '@/types';

export function PlannedTransactionsCombo({
  fieldValue,
  onChange,
  setFormValue,
  disabled = false,
}: {
  fieldValue: string | null;
  onChange: (value: string | null) => void;
  setFormValue: (plannedTransaction: PlannedTransactionDto) => void;
  className?: string;
  disabled?: boolean;
}) {
  const t = useTranslations(
    'Pages.Transactions.Components.PlannedTransactionCombo'
  );
  const [open, setOpen] = useState(false);

  const { data, isLoading, isError, failureReason } =
    useFetchPlannedTransactionsPaginated();

  if (isError) {
    return <p>Error: {failureReason?.message}</p>;
  }

  var plannedTransactions = data?.data ?? [];

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <FormControl className='w-full'>
        <PopoverTrigger asChild>
          <Button
            disabled={isLoading || disabled}
            variant='outline'
            role='combobox'
            aria-expanded={open}
            className={cn(
              'justify-between',
              !fieldValue && 'text-muted-foreground'
            )}
          >
            {fieldValue ? (
              <p className='truncate'>
                {
                  plannedTransactions.find(
                    (transaction) => transaction.id === fieldValue
                  )?.description
                }
              </p>
            ) : (
              <p className='truncate'>{t('DefaultMessage')}</p>
            )}
            {isLoading ? (
              <Loader2 className='ml-2 size-4 animate-spin' />
            ) : (
              <ChevronsUpDown className='ml-2 size-4 shrink-0 opacity-50' />
            )}
          </Button>
        </PopoverTrigger>
      </FormControl>
      <PopoverContent className='p-0' align='start'>
        <Command>
          <CommandInput placeholder={t('SearchMessage')} />
          <CommandEmpty>{t('EmptyMessage')}</CommandEmpty>
          <CommandGroup>
            <CommandList>
              {plannedTransactions.map((transaction) => (
                <CommandItem
                  value={transaction.description}
                  key={transaction.id}
                  onSelect={() => {
                    onChange(
                      transaction.id === fieldValue ? null : transaction.id
                    );
                    setFormValue(transaction);
                    setOpen(false);
                  }}
                >
                  <Check
                    className={cn(
                      'mr-2 size-4',
                      transaction.id === fieldValue
                        ? 'opacity-100'
                        : 'opacity-0'
                    )}
                  />
                  {transaction.description}
                </CommandItem>
              ))}
            </CommandList>
          </CommandGroup>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
