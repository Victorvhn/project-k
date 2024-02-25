'use client';

import { useCallback, useEffect, useState, useTransition } from 'react';
import { addMonths } from 'date-fns';
import { CalendarIcon } from 'lucide-react';
import { useParams } from 'next/navigation';
import { useFormatter, useTranslations } from 'next-intl';
import nProgress, * as NProgress from 'nprogress';

import { usePathname, useRouter } from '@/lib/navigation';
import { cn } from '@/lib/utils';

import { Button } from './ui/button';
import { Popover, PopoverContent, PopoverTrigger } from './ui/popover';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from './ui/select';
import { Separator } from './ui/separator';
import { Skeleton } from './ui/skeleton';

const availableMonths = [
  { value: 1, label: 'January' },
  { value: 2, label: 'February' },
  { value: 3, label: 'March' },
  { value: 4, label: 'April' },
  { value: 5, label: 'May' },
  { value: 6, label: 'June' },
  { value: 7, label: 'July' },
  { value: 8, label: 'August' },
  { value: 9, label: 'September' },
  { value: 10, label: 'October' },
  { value: 11, label: 'November' },
  { value: 12, label: 'December' },
] as {
  value: number;
  label:
    | 'January'
    | 'February'
    | 'March'
    | 'April'
    | 'May'
    | 'June'
    | 'July'
    | 'August'
    | 'September'
    | 'October'
    | 'November'
    | 'December';
}[];

export function MonthPicker() {
  const t = useTranslations('Components.MonthPicker');
  const params = useParams();
  const [mounted, setMounted] = useState(false);
  const [month] = useState<string>(() => {
    return params.date ? (params.date as string).split('-')[1] : '';
  });
  const [year] = useState<string>(() => {
    return params.date ? (params.date as string).split('-')[0] : '';
  });
  const [availableYears] = useState<{ value: number; label: string }[]>(() => {
    const yearNow = new Date().getFullYear();

    const state = [];
    for (let index = yearNow - 5; index < yearNow + 10; index++) {
      state.push({ value: index, label: index.toString() });
    }

    return state;
  });
  const format = useFormatter();

  const pathname = usePathname();
  const router = useRouter();
  const [isPending, startTransition] = useTransition();

  useEffect(() => {
    setMounted(true);
  }, []);

  useEffect(() => {
    if (nProgress.isStarted()) {
      NProgress.done();
    }
  }, [pathname]);

  const onSelectChange = useCallback(
    (value: string) => {
      if (value === `${year}-${month}`) {
        return;
      }

      startTransition(() => {
        NProgress.start();
        router.push(`/dashboard/${value}`);
      });
    },
    [router, year, month]
  );

  const handleMonthChange = (value: string) => {
    onSelectChange(`${year}-${value}`);
  };

  const handleYearChange = (value: string) => {
    onSelectChange(`${value}-${month}`);
  };

  const handleButtonClick = (value: number) => {
    const currentDate = new Date();
    const newDate = addMonths(currentDate, value);

    onSelectChange(`${newDate.getFullYear()}-${newDate.getMonth() + 1}`);
  };

  if (!mounted) {
    return <Skeleton className='h-10 w-64' />;
  }

  return (
    <div className='grid gap-2'>
      <Popover>
        <PopoverTrigger asChild>
          <Button
            id='date'
            variant={'outline'}
            className={cn(
              'w-64 justify-start text-left font-normal',
              !month && !year ? 'text-muted-foreground' : ''
            )}
            disabled={isPending}
          >
            <CalendarIcon className='mr-2 h-4 w-4' />
            {month && year ? (
              <span className='capitalize-first'>
                {format.dateTime(new Date(Number(year), Number(month) - 1, 1), {
                  year: 'numeric',
                  month: 'long',
                })}
              </span>
            ) : (
              <span>{t('DefaultMessage')}</span>
            )}
          </Button>
        </PopoverTrigger>
        <PopoverContent className='flex w-64 flex-col space-y-2 p-0 p-3'>
          <div className='flex space-x-2'>
            <Select onValueChange={handleMonthChange} value={month}>
              <SelectTrigger>
                <SelectValue placeholder='Month' />
              </SelectTrigger>
              <SelectContent>
                {availableMonths.map((month) => (
                  <SelectItem key={month.value} value={month.value.toString()}>
                    {t(`Months.${month.label}`)}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
            <Select onValueChange={handleYearChange} value={year}>
              <SelectTrigger>
                <SelectValue placeholder='Year' />
              </SelectTrigger>
              <SelectContent>
                {availableYears.map((year) => (
                  <SelectItem key={year.value} value={year.value.toString()}>
                    {year.label}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
          <Separator orientation='horizontal' />
          <Button variant='outline' onClick={() => handleButtonClick(-1)}>
            {t('PreviousMonth')}
          </Button>
          <Button variant='outline' onClick={() => handleButtonClick(0)}>
            {t('CurrentMonth')}
          </Button>
          <Button variant='outline' onClick={() => handleButtonClick(1)}>
            {t('NextMonth')}
          </Button>
        </PopoverContent>
      </Popover>
    </div>
  );
}
