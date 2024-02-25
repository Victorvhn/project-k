'use client';

import { useState } from 'react';
import { Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { useDeleteUser } from '@/data/users/use-delete-user';
import { useMediaQuery } from '@/hooks/use-media-query';

import { Button } from './ui/button';
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from './ui/dialog';
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from './ui/drawer';

export default function DeleteAccountButton() {
  const [isLoading, setIsLoading] = useState(false);

  const t = useTranslations('Components.DeleteAccountButton');
  const isSmallDevice = useMediaQuery('only screen and (max-width : 768px)');

  const mutation = useDeleteUser();

  const handleClick = () => {
    setIsLoading(true);
    mutation.mutate(undefined, { onSettled: () => setIsLoading(false) });
  };

  if (!isSmallDevice) {
    return (
      <Dialog>
        <DialogTrigger asChild>
          <Button variant='destructive'>{t('DeleteAccount')}</Button>
        </DialogTrigger>
        <DialogContent
          className='sm:max-w-[425px]'
          onEscapeKeyDown={(event) => event.preventDefault()}
          onPointerDownOutside={(event) => event.preventDefault()}
        >
          <DialogHeader>
            <DialogTitle>{t('Title')}</DialogTitle>
            <DialogDescription>{t('Description')}</DialogDescription>
          </DialogHeader>
          <Button
            variant='destructive'
            onClick={handleClick}
            disabled={isLoading}
            tabIndex={-1}
          >
            {isLoading ? (
              <>
                <Loader2 className='mr-2 size-4 animate-spin' />
                {t('Deleting')}
              </>
            ) : (
              t('Yes')
            )}
          </Button>
          <DialogClose asChild>
            <Button variant='default' disabled={isLoading}>
              {t('Cancel')}
            </Button>
          </DialogClose>
        </DialogContent>
      </Dialog>
    );
  }

  return (
    <Drawer>
      <DrawerTrigger asChild>
        <Button variant='destructive'>{t('Title')}</Button>
      </DrawerTrigger>
      <DrawerContent
        onEscapeKeyDown={(event) => event.preventDefault()}
        onPointerDownOutside={(event) => event.preventDefault()}
      >
        <DrawerHeader className='text-left'>
          <DrawerTitle>{t('Title')}</DrawerTitle>
          <DrawerDescription>{t('Description')}</DrawerDescription>
        </DrawerHeader>
        <DrawerFooter className='pt-2'>
          <Button
            variant='destructive'
            onClick={handleClick}
            disabled={isLoading}
          >
            {t('Yes')}
          </Button>
          <DrawerClose asChild>
            <Button variant='default' disabled={isLoading}>
              {t('Cancel')}
            </Button>
          </DrawerClose>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
}
