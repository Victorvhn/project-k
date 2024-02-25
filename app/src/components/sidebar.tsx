'use client';

import { useEffect, useState } from 'react';
import { LayoutDashboard, ListIcon, MenuIcon, X } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { Link, usePathname } from '@/lib/navigation';
import { cn } from '@/lib/utils';

import { Button } from './ui/button';
import {
  Drawer,
  DrawerContent,
  DrawerFooter,
  DrawerHeader,
  DrawerTrigger,
} from './ui/drawer';
import { Separator } from './ui/separator';
import ChangeCurrency from './change-currency';
import DeleteAccountButton from './delete-account-button';

type AvailablePages = 'dashboard' | 'categories' | null;

export default function Sidebar() {
  const t = useTranslations('Components.Sidebar');
  const [isOpen, setIsOpen] = useState(false);
  const [currentPage, setCurrentPage] = useState<AvailablePages>('dashboard');
  const path = usePathname();

  useEffect(() => {
    const split = path.split('/').filter((f) => !!f);

    if (split[0] === 'dashboard') {
      setCurrentPage('dashboard');
    } else if (split[0] === 'categories' && split[1] === 'list') {
      setCurrentPage('categories');
    } else {
      setCurrentPage(null);
    }
  }, [path]);

  return (
    <Drawer direction='left' open={isOpen} onClose={() => setIsOpen(false)}>
      <DrawerTrigger onClick={() => setIsOpen((old) => !old)}>
        <MenuIcon className='size-6' />
      </DrawerTrigger>
      <DrawerContent
        onEscapeKeyDown={() => setIsOpen((old) => !old)}
        onPointerDownOutside={() => setIsOpen((old) => !old)}
      >
        <DrawerHeader className='flex items-center justify-between'>
          <span className='ml-4 font-bold'>Project K</span>
          <Button variant='ghost' size='icon' onClick={() => setIsOpen(false)}>
            <X className='size-5 text-muted-foreground' />
          </Button>
        </DrawerHeader>
        <Separator />
        <div className='mt-4 flex w-full flex-col items-center space-y-2 px-4'>
          <Button
            variant='ghost'
            className={cn(
              'w-full justify-start',
              currentPage === 'dashboard' &&
                'bg-blue-100 text-blue-800 hover:bg-blue-200 hover:text-blue-900'
            )}
            onClick={() => setIsOpen(false)}
            asChild
          >
            <Link href='/'>
              <LayoutDashboard className='mr-4 size-5' />
              {t('Dashboard')}
            </Link>
          </Button>
          <Button
            variant='ghost'
            className={cn(
              'w-full justify-start',
              currentPage === 'categories' &&
                'bg-blue-100 text-blue-800 hover:bg-blue-200 hover:text-blue-900'
            )}
            onClick={() => setIsOpen(false)}
            asChild
          >
            <Link href='/categories/list'>
              <ListIcon className='mr-4 size-5' />
              {t('Categories')}
            </Link>
          </Button>
        </div>
        <DrawerFooter>
          <ChangeCurrency />
          <DeleteAccountButton />
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
}
