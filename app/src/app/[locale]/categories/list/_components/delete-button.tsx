'use client';

import { useState } from 'react';
import { Loader2, TrashIcon } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { ByDeviceModal } from '@/components/by-device-modal';
import { Button } from '@/components/ui/button';
import { useDeleteCategory } from '@/data/categories/delete-category';

export function DeleteButton({ categoryId }: { categoryId: string }) {
  const [isLoading, setIsLoading] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const t = useTranslations('Pages.Categories.List.DeleteButton');

  const deleteMutation = useDeleteCategory();

  const handleClick = () => {
    setIsLoading(true);
    deleteMutation.mutate(
      {
        id: categoryId,
      },
      {
        onSuccess: () => {
          setIsOpen(false);
        },
        onSettled: () => setIsLoading(false),
      }
    );
  };

  return (
    <ByDeviceModal
      open={isOpen}
      title={t('Title')}
      description={t('Description')}
      trigger={
        <Button
          variant='ghost'
          className='ml-2 size-8 p-0'
          onClick={() => setIsOpen((old) => !old)}
        >
          <TrashIcon className='size-4' />
        </Button>
      }
    >
      <div className='flex flex-col space-y-2'>
        <Button
          disabled={isLoading}
          aria-label='Delete button'
          onClick={handleClick}
        >
          {t('Delete')}
        </Button>
        {isLoading && (
          <div className='mt-2 flex flex-row items-center justify-center text-muted-foreground'>
            <Loader2 className='mr-2 h-4 w-4 animate-spin' />
            <p>{t('Deleting')}</p>
          </div>
        )}
      </div>
    </ByDeviceModal>
  );
}
