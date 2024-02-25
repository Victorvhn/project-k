import React from 'react';
import { ResizableBox as ReactResizableBox } from 'react-resizable';

import { cn } from '@/lib/utils';

import 'react-resizable/css/styles.css';

export default function ResizableBox({
  children,
  width = 1150,
  height = 630,
  className = '',
}: {
  children: React.ReactNode;
  width?: number;
  height?: number;
  className?: string;
}) {
  return (
    <div className='inline-block w-auto'>
      <ReactResizableBox width={width} height={height}>
        <div className={cn('h-full w-full', className)}>{children}</div>
      </ReactResizableBox>
    </div>
  );
}
