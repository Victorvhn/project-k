import { unstable_setRequestLocale } from 'next-intl/server';

import { redirect } from '@/lib/navigation';
import { Params } from '@/types';

export default async function Index({
  params: { locale },
}: {
  params: Params;
}) {
  unstable_setRequestLocale(locale);

  const date = new Date(Date.now());
  const dateInString = `${date.getFullYear()}-${date.getMonth() + 1}`;
  redirect(`/dashboard/${dateInString}`);
}
