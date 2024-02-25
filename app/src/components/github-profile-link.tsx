import { GithubIcon } from 'lucide-react';
import Link from 'next/link';
import { getTranslations } from 'next-intl/server';

import { Button } from './ui/button';
import { Tooltip, TooltipContent, TooltipTrigger } from './ui/tooltip';

export default async function GithubProfileLink() {
  const t = await getTranslations('Components.GithubProfileLink');

  return (
    <Tooltip>
      <TooltipTrigger asChild>
        <Button
          variant='ghost'
          size='icon'
          aria-label={t('ButtonDescription')}
          asChild
        >
          <Link
            href='https://github.com/Victorvhn'
            target='_blank'
            rel='noreferrer'
          >
            <GithubIcon />
          </Link>
        </Button>
      </TooltipTrigger>
      <TooltipContent align='end'>
        <p>{t('ButtonDescription')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
