import { forwardRef } from 'react';
import { NumericFormat } from 'react-number-format';
import { useSession } from 'next-auth/react';

import { cn } from '@/lib/utils';
import { getDefaultNumberFormat } from '@/shared/get-separator';

export interface CurrencyInputProps {
  className?: string;
}

const CurrencyInput = forwardRef<HTMLInputElement, CurrencyInputProps>(
  ({ className, ...props }, ref) => {
    const session = useSession();
    const format = getDefaultNumberFormat(
      session.data?.preferred_currency ?? 'BRL'
    );

    return (
      <NumericFormat
        inputMode='decimal'
        allowNegative={false}
        allowLeadingZeros={false}
        thousandSeparator={format.thousandSeparator}
        decimalSeparator={format.decimalSeparator}
        prefix={format.currencySymbol}
        fixedDecimalScale
        decimalScale={2}
        className={cn(
          'flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50',
          className
        )}
        getInputRef={ref}
        {...props}
      />
    );
  }
);
CurrencyInput.displayName = 'CurrencyInput';

export { CurrencyInput };
