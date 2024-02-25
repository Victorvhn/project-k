'use client';

import { Dispatch, SetStateAction, useEffect } from 'react';
import { useSession } from 'next-auth/react';

export default function RefreshTokenHandler({
  setRefetchInterval,
}: {
  setRefetchInterval: Dispatch<SetStateAction<number>>;
}) {
  const { data: session } = useSession();

  useEffect(() => {
    if (!!session) {
      const timeRemaining = Math.round(
        (session.api_access_token_expires_at - Date.now()) / 60000 - 1
      );
      setRefetchInterval(timeRemaining > 0 ? timeRemaining : 0);
    }
  }, [session, setRefetchInterval]);

  return null;
}
