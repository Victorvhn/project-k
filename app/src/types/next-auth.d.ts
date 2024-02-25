import { JWT } from "next-auth/jwt"
import NextAuth from "next-auth"

declare module 'next-auth' {
  interface Session {
    api_access_token: string;
    api_access_token_expires_at: number;
    user_id: string;
    preferred_currency: 'USD' | 'BRL';
  }
}

declare module 'next-auth/jwt' {
  interface JWT {
    api_access_token: string;
    api_access_token_expires_at: number;
    user_id: string;
    preferred_currency: 'USD' | 'BRL';
  }
}
