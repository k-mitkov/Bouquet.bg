import { Spinner } from '@/components/utilities';
import { useUser } from '../stateProviders/userContext/UserContext';
import { Navigate } from 'react-router';

export function ProtectedRoute({ children, requiredClaims }: { children: JSX.Element, requiredClaims: string[] }) {
  const { user, isUserLoaded } = useUser();

  if(!isUserLoaded){
    return (
      <div className='flex w-full overflow-hidden items-center justify-center'>
        <Spinner />
      </div>
    )
  }

  const hasRequiredClaims = requiredClaims.every((claim) =>
    user?.claims.includes(claim)
  );

  if (!hasRequiredClaims) {
    return <Navigate to="/" replace />;
  }

  return children;
}
