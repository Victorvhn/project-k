import { ApiHealthCheckStatus } from '..';

export type ApiHealthCheckResponse = {
  status: ApiHealthCheckStatus;
  totalDuration: string;
  entries: {
    npgsql: {
      duration: string;
      status: ApiHealthCheckStatus;
    };
  };
};
