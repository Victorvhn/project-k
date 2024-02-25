export type ProblemDetails = {
  type: string;
  title: string;
  status: number;
  detail?: string;
  instance: string;
  errors?: string[] | { [unknown: string]: string[] };
};
