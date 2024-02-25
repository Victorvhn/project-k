import { getServerSession } from 'next-auth';

import { Link } from '@/lib/navigation';

import GithubProfileLink from './github-profile-link';
import LanguageSwitch from './language-switch';
import Sidebar from './sidebar';
import SignOutButton from './sign-out-button';
import ThemeSwitch from './theme-switch';

export async function Header() {
  const session = await getServerSession();

  return (
    <header className='sticky top-0 z-50 w-full border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60'>
      <div className='flex h-14 items-center justify-between px-6'>
        <div className='flex items-center space-x-2'>
          {session?.user && <Sidebar />}
          <Link href='/' className='text-2xl font-bold'>
            Project K
          </Link>
        </div>

        <div className='flex space-x-2'>
          <GithubProfileLink />
          <LanguageSwitch />
          <ThemeSwitch />
          {session?.user && <SignOutButton />}
        </div>
      </div>
    </header>
  );
}
