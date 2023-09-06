import { Token } from "../../resources";

export interface User {
    id: string;
    tokens: Token;
    claims: string[];
    userImageUrl: string;
}

export interface UserContextType {
    user: User | null,
    setUser: React.Dispatch<React.SetStateAction<User>>,
    logout: () => void,
    isAuthenticated: () => boolean,
    isUserLoaded: boolean;
}