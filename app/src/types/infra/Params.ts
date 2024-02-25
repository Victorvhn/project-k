import { locales } from '@/shared/supported-languages';

export type Params = {
  locale: (typeof locales)[number];
};

export interface DashboardParams extends Params {
  date: string;
}
