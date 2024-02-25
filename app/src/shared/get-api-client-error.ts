import { ProblemDetails } from '@/types';

export function getApiErrorTitle(
  errorMessage: string | undefined
): string | null {
  if (!errorMessage) {
    return null;
  }

  let problemDetails: ProblemDetails;
  try {
    problemDetails = JSON.parse(errorMessage) as ProblemDetails;
  } catch {
    return 'Could not parse error message';
  }

  return problemDetails.title;
}

export function getApiErrorStatus(
  errorMessage: string | undefined
): number | null {
  if (!errorMessage) {
    return null;
  }

  let problemDetails: ProblemDetails;
  try {
    problemDetails = JSON.parse(errorMessage) as ProblemDetails;
  } catch {
    return 500;
  }

  return problemDetails.status;
}

export function getApiErrorDetails(
  errorMessage: string | undefined
): string[] | null {
  if (!errorMessage) {
    return null;
  }

  let problemDetails: ProblemDetails;
  try {
    problemDetails = JSON.parse(errorMessage) as ProblemDetails;
  } catch {
    return ['Could not parse error message'];
  }

  let errors: string[] = [];

  if (problemDetails.detail) {
    errors = errors.concat(problemDetails.detail.split('\n'));
  }

  if (problemDetails.errors) {
    if (Array.isArray(problemDetails.errors)) {
      errors = errors.concat(problemDetails.errors);
    } else if (typeof problemDetails.errors === 'object') {
      Object.values(problemDetails.errors).forEach((errorArray) => {
        errors = errors.concat(errorArray);
      });
    }
  }

  return errors;
}
