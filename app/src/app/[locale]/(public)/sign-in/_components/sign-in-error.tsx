'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';

import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
} from '@/components/ui/drawer';
import { useMediaQuery } from '@/hooks/use-media-query';
import { Link } from '@/lib/navigation';

export default function SignInError() {
  const t = useTranslations('Pages.SignIn.Components.Error');
  const [open, setOpen] = useState(true);
  const isSmallDevice = useMediaQuery('only screen and (max-width : 768px)');

  if (!isSmallDevice) {
    return (
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent className='sm:max-w-[425px]'>
          <DialogHeader>
            <DialogTitle>{t('Title')}</DialogTitle>
            <DialogDescription>{t('Description')}</DialogDescription>
          </DialogHeader>
          <Button variant='default' asChild>
            <Link href='/server-status'>{t('CheckServerStatus')}</Link>
          </Button>
        </DialogContent>
      </Dialog>
    );
  }

  return (
    <Drawer open={open} onOpenChange={setOpen}>
      <DrawerContent>
        <DrawerHeader className='text-left'>
          <DrawerTitle>{t('Title')}</DrawerTitle>
          <DrawerDescription>{t('Description')}</DrawerDescription>
        </DrawerHeader>
        <DrawerFooter className='pt-2'>
          <Button variant='default' asChild>
            <Link href='/server-status'>{t('CheckServerStatus')}</Link>
          </Button>
          <DrawerClose asChild>
            <Button variant='outline'>{t('Cancel')}</Button>
          </DrawerClose>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
}
