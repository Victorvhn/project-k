export function transformCurrencyToNumber(stringCurrency: string): number {
  if (stringCurrency[0] !== 'R' && stringCurrency[0] !== '$') {
    // If the string doesn't start with R$ or $, it's already a number
    return Number(stringCurrency);
  }

  let cleanString = stringCurrency.replace(/(R\$|\$|\s)/g, '');

  const lastPart = cleanString.slice(-3);

  const hasDecimalSeparator = lastPart.includes(',') || lastPart.includes('.');

  if (!hasDecimalSeparator) {
    cleanString = cleanString.replace(/[,.]/g, '');
  } else {
    const firstPart = cleanString.slice(0, -3);
    cleanString =
      firstPart.replace(/[,.]/g, '') + lastPart.replace(/[,]/g, '.');
  }

  return Number(cleanString);
}
