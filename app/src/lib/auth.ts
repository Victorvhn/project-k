import jsonwebtoken from 'jsonwebtoken';
import { NextAuthOptions } from 'next-auth';
import { JWT } from 'next-auth/jwt';
import Github from 'next-auth/providers/github';
import Google from 'next-auth/providers/google';

import { createUser, userExists } from '@/services/users';
import { UserDto } from '@/types';

function getNewApiAccessToken(
  nextAuthToken: JWT,
  userId: string
): {
  apiAccessToken: string;
  apiTokenExpiresAt: number;
} {
  const encodedToken = jsonwebtoken.sign(
    {
      emailaddress: nextAuthToken.email,
      name: nextAuthToken.name,
    },
    process.env.JWT_SECRET_KEY!,
    {
      expiresIn: '1h',
      subject: userId,
      issuer: 'ProjectKWebApp',
      mutatePayload: false,
      audience: 'ProjectK',
      notBefore: 0,
    }
  );

  return {
    apiAccessToken: encodedToken,
    apiTokenExpiresAt: Date.now() + 60 * 60 * 1000, // 1 hour
  };
}

export const authOptions: NextAuthOptions = {
  pages: {
    signIn: '/sign-in',
  },
  session: {
    strategy: 'jwt',
  },
  secret: process.env.NEXTAUTH_SECRET!,
  providers: [
    Google({
      clientId: process.env.GOOGLE_ID!,
      clientSecret: process.env.GOOGLE_SECRET!,
    }),
    Github({
      clientId: process.env.GITHUB_ID!,
      clientSecret: process.env.GITHUB_SECRET!,
    }),
  ],
  callbacks: {
    jwt: async ({ token, trigger, session }) => {
      if (trigger === 'signIn') {
        if (!token.name || !token.email) {
          throw new Error("Couldn't check user");
        }

        const userExistsResult = await userExists(token.email!);

        let userId;
        let currency: 'USD' | 'BRL' = 'BRL';
        if (!userExistsResult.ok) {
          const createUserResult = await createUser({
            name: token.name!,
            email: token.email!,
          });

          if (!createUserResult) {
            throw new Error("Couldn't create user");
          }

          const data = (await createUserResult.json()) as UserDto;

          userId = data.id;
          currency = data.currency.toUpperCase() as 'USD' | 'BRL';
        } else {
          const data = (await userExistsResult.json()) as UserDto;
          userId = data.id;
          currency = data.currency.toUpperCase() as 'USD' | 'BRL';
        }

        const { apiAccessToken, apiTokenExpiresAt } = getNewApiAccessToken(
          token,
          userId
        );

        token.user_id = userId;
        token.api_access_token = apiAccessToken;
        token.api_access_token_expires_at = apiTokenExpiresAt;
        token.preferred_currency = currency;

        return Promise.resolve(token);
      }

      if (trigger === 'update' && session) {
        token.preferred_currency = session.preferred_currency;
      }

      // Calculating the remaining time until the token expires in minutes and subtracting 5 minutes as a safety buffer
      const shouldRefreshToken = Math.round(
        // Subtracting the current timestamp from the token's expiration timestamp and converting milliseconds to minutes
        // Then deducting an additional 5 minutes to ensure token refresh before it actually expires
        (token.api_access_token_expires_at - Date.now()) / 60000 - 5
      );

      if (shouldRefreshToken > 0) {
        return Promise.resolve(token);
      }

      const { apiAccessToken, apiTokenExpiresAt } = getNewApiAccessToken(
        token,
        token.user_id
      );
      token.api_access_token = apiAccessToken;
      token.api_access_token_expires_at = apiTokenExpiresAt;

      return Promise.resolve(token);
    },
    session: async ({ session, token }) => {
      session.user_id = token.user_id;
      session.api_access_token = token.api_access_token;
      session.api_access_token_expires_at = token.api_access_token_expires_at;
      session.preferred_currency = token.preferred_currency;

      return Promise.resolve(session);
    },
  },
};
