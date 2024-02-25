'use client';

import { useEffect, useTransition } from 'react';
import { useSearchParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import * as NProgress from 'nprogress';

import { Button, ButtonProps } from '@/components/ui/button';
import { usePathname, useRouter } from '@/lib/navigation';

export default function BackButton({
  children,
  onClick,
  ...rest
}: ButtonProps) {
  const t = useTranslations('Shared');

  const pathname = usePathname();
  const searchParams = useSearchParams();
  const router = useRouter();
  const [isPending, startTransition] = useTransition();

  useEffect(() => {
    if (NProgress.isStarted()) {
      NProgress.done();
    }
  }, [pathname, searchParams]);

  function handleClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
    if (onClick) {
      onClick(event);
    }
    startTransition(() => {
      NProgress.start();
      router.back();
    });
  }

  return (
    <Button
      onClick={(event) => handleClick(event)}
      disabled={isPending}
      aria-label='Back button'
      {...rest}
    >
      {children ?? t('Cancel')}
    </Button>
  );
}
