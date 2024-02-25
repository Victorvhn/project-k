export function getDefaultNumberFormat(currencyCode: 'USD' | 'BRL') {
  let thousandSeparator;
  let decimalSeparator;
  let currencySymbol;

  switch (currencyCode) {
    case 'USD':
      thousandSeparator = ',';
      decimalSeparator = '.';
      currencySymbol = '$';
      break;
    case 'BRL':
      thousandSeparator = '.';
      decimalSeparator = ',';
      currencySymbol = 'R$ ';
      break;
  }

  return {
    thousandSeparator,
    decimalSeparator,
    currencySymbol,
  };
}
