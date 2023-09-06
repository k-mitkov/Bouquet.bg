import { Outlet } from 'react-router-dom';
import { useUser } from '../stateProviders/userContext/UserContext';
import { Navigate } from 'react-router';
import { Spinner } from '@/components/utilities';

export function AuthenticatedRoute() {
  const { isAuthenticated, isUserLoaded } = useUser();

  if (!isUserLoaded) {
    return (
      <div className='flex w-full overflow-hidden items-center justify-center'>
        <Spinner />
      </div>
    )
  }

  if (isAuthenticated()) {
    return <Outlet />
  }
  return <Navigate to="/login" replace />;
}