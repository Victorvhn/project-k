import SummaryCardSkeleton from './components/skeleton';

export default async function MonthlySummaryLoading() {
  return (
    <>
      <SummaryCardSkeleton />
      <SummaryCardSkeleton />
    </>
  );
}
