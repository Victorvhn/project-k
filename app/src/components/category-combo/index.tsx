'use client';

import { useState } from 'react';
import { Check, ChevronsUpDown, Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { useFetchCategories } from '@/data/categories/fetch-categories';
import { cn } from '@/lib/utils';

import { Button } from '../ui/button';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
} from '../ui/command';
import { FormControl } from '../ui/form';
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover';

import { CreateCategoryButton } from './create-category-button';

export function CategoryCombo({
  fieldValue,
  onChange,
  disabled = false,
}: {
  fieldValue: string | null;
  onChange: (value: string | null) => void;
  className?: string;
  disabled?: boolean;
}) {
  const t = useTranslations('Components.CategoryCombo');
  const [open, setOpen] = useState(false);

  const { data, isLoading, isError, failureReason } = useFetchCategories();

  if (isError) {
    return <p>Error: {failureReason?.message}</p>;
  }

  var categories = data?.data ?? [];

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <div className='flex w-full space-x-2'>
        <div className='flex w-full justify-between'>
          <PopoverTrigger asChild>
            <FormControl>
              <Button
                disabled={isLoading || disabled}
                variant='outline'
                role='combobox'
                aria-expanded={open}
                className={cn(
                  'w-[calc(100%-48px)] justify-between',
                  !fieldValue && 'text-muted-foreground'
                )}
              >
                {fieldValue ? (
                  <div className='flex items-center space-x-2 truncate'>
                    <div
                      className='size-4 rounded-full'
                      style={{
                        backgroundColor: categories.find(
                          (category) => category.id === fieldValue
                        )?.hexColor,
                      }}
                    />
                    {
                      <p className='w-[calc(100%-24px)] truncate text-left'>
                        {
                          categories.find(
                            (category) => category.id === fieldValue
                          )?.name
                        }
                      </p>
                    }
                  </div>
                ) : (
                  <p className='truncate'>{t('DefaultMessage')}</p>
                )}
                {isLoading ? (
                  <Loader2 className='ml-2 size-4 animate-spin' />
                ) : (
                  <ChevronsUpDown className='ml-2 size-4 shrink-0 opacity-50' />
                )}
              </Button>
            </FormControl>
          </PopoverTrigger>
          <CreateCategoryButton disabled={isLoading || disabled} />
        </div>
      </div>
      <PopoverContent className='w-[90%] p-0' align='start'>
        <Command>
          <CommandInput placeholder={t('SearchMessage')} />
          <CommandEmpty>{t('EmptyMessage')}</CommandEmpty>
          <CommandGroup>
            {categories.map((category) => (
              <CommandItem
                value={category.name}
                key={category.id}
                onSelect={() => {
                  onChange(category.id === fieldValue ? null : category.id);
                  setOpen(false);
                }}
              >
                <Check
                  className={cn(
                    'mr-2 size-4',
                    category.id === fieldValue ? 'opacity-100' : 'opacity-0'
                  )}
                />
                {category.name}
              </CommandItem>
            ))}
          </CommandGroup>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
