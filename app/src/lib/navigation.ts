import { createSharedPathnamesNavigation } from 'next-intl/navigation';

import { locales } from '@/shared/supported-languages';

export const { Link, redirect, usePathname, useRouter } =
  createSharedPathnamesNavigation({ locales: locales });
