import { expect, test } from '@playwright/test';

test.describe('Testing page', () => {
  test('It should work', async ({ page }) => {
    await page.goto('/');

    await expect(page).toHaveURL(`/sign-in`);
  });
});
