import { vi } from 'vitest';
import React from 'react';

vi.mock('react-router-dom', () => ({
    ...vi.importMock('react-router-dom'),
    useHistory: vi.fn(),
    useParams: vi.fn(),
    useLocation: () => ({
        search: '',
        pathname: '/',
    }),
    useNavigate: vi.fn(),
    matchPath: vi.fn(),
    withRouter: vi.fn(),
    useRouteMatch: vi.fn(),
    Link: ({ children, to }: { children: JSX.Element; to: string }) =>
        React.createElement('a', { href: to }, children),
    Router: () => vi.fn(),
    HashRouter: () => vi.fn(),
    Switch: () => vi.fn(),
}));