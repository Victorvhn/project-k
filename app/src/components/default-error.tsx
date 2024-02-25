import * as ScrollAreaPrimitive from '@radix-ui/react-scroll-area';

import { cn } from '@/lib/utils';
import {
  getApiErrorStatus,
  getApiErrorTitle,
} from '@/shared/get-api-client-error';

import { Separator } from './ui/separator';

interface ErrorCardProps
  extends React.ComponentPropsWithoutRef<typeof ScrollAreaPrimitive.Root> {
  failureReason: Error | null;
}

export default function DefaultError({
  failureReason,
  className,
  ...rest
}: ErrorCardProps) {
  const title = getApiErrorTitle(failureReason?.message);
  const statusCode = getApiErrorStatus(failureReason?.message);

  return (
    <div
      className={cn(
        'flex h-full w-full items-center justify-center',
        className
      )}
      {...rest}
    >
      <div className='flex h-[30px] flex-row items-center justify-center space-x-4'>
        <p className='text-xl font-bold'>{statusCode}</p>
        <Separator orientation='vertical' />
        <p className='text-md font-light'>{title}</p>
      </div>
    </div>
  );
}
