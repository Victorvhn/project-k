import { z } from 'zod';

export const CategoryFormSchema = z.object({
  name: z
    .string({
      required_error: 'Pages.Categories.Form.Validations.NameRequired',
    })
    .min(2, { message: 'Pages.Categories.Form.Validations.NameMinLength' })
    .max(50, { message: 'Pages.Categories.Form.Validations.NameMaxLength' }),
  color: z.string({
    required_error: 'Pages.Categories.Form.Validations.ColorRequired',
  }),
});
