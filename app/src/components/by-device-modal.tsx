import React, { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';

import { useMediaQuery } from '@/hooks/use-media-query';

import { Button } from './ui/button';
import {
  Dialog,
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

export interface ByDeviceModalProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  trigger: React.ReactNode;
  title: string;
  description: string;
  children: React.ReactNode;
  open?: boolean;
}

const ByDeviceModal = React.forwardRef<HTMLButtonElement, ByDeviceModalProps>(
  (
    { trigger, title, description, children, open: parentOpen = false },
    ref
  ) => {
    const [open, setOpen] = useState(false);

    useEffect(() => {
      setOpen(parentOpen);
    }, [parentOpen]);

    const t = useTranslations('Shared');
    const isSmallDevice = useMediaQuery('only screen and (max-width : 768px)');

    if (isSmallDevice) {
      return (
        <Drawer open={open} onOpenChange={setOpen}>
          <DrawerTrigger asChild ref={ref}>
            {trigger}
          </DrawerTrigger>
          <DrawerContent>
            <DrawerHeader className='text-left'>
              <DrawerTitle>{title}</DrawerTitle>
              <DrawerDescription>{description}</DrawerDescription>
            </DrawerHeader>
            <div className='mb-4 w-[90%] justify-center self-center'>
              {children}
            </div>
            <DrawerFooter className='pt-2'>
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
        <DialogTrigger asChild ref={ref}>
          {trigger}
        </DialogTrigger>
        <DialogContent className='sm:max-w-[425px]'>
          <DialogHeader>
            <DialogTitle>{title}</DialogTitle>
            <DialogDescription>{description}</DialogDescription>
          </DialogHeader>
          {children}
        </DialogContent>
      </Dialog>
    );
  }
);
ByDeviceModal.displayName = 'ByDeviceModal';

export { ByDeviceModal };
