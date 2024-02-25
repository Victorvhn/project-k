'use client';

import { useEffect, useTransition } from 'react';
import { useFormContext } from 'react-hook-form';
import { PlusIcon } from 'lucide-react';
import { useSearchParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import * as NProgress from 'nprogress';

import { usePathname, useRouter } from '@/lib/navigation';

import { Button } from '../ui/button';
import { Tooltip, TooltipContent, TooltipTrigger } from '../ui/tooltip';

export function CreateCategoryButton({
  disabled = false,
}: {
  disabled?: boolean;
}) {
  const t = useTranslations('Components.CategoryCombo');

  const { getValues } = useFormContext();

  const pathname = usePathname();
  const searchParams = useSearchParams();
  const router = useRouter();
  const [isPending, startTransition] = useTransition();

  useEffect(() => {
    if (NProgress.isStarted()) {
      NProgress.done();
    }
  }, [pathname, searchParams]);

  function handleClick() {
    const formData = getValues();

    sessionStorage.setItem('form-data', JSON.stringify(formData));

    startTransition(() => {
      NProgress.start();
      router.push('/categories/add');
    });
  }

  return (
    <Tooltip>
      <TooltipTrigger asChild>
        <Button
          size='icon'
          variant='outline'
          disabled={isPending || disabled}
          onClick={handleClick}
          role='a'
          type='button'
        >
          <PlusIcon className='size-4' />
        </Button>
      </TooltipTrigger>
      <TooltipContent align='end'>
        <p>{t('CreateTooltip')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
