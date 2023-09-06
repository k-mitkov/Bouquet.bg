import { createContext, useContext } from "react";
import { UserContextType } from "./UserTypes";


export const UserContext = createContext<UserContextType>({
    user: {
        id: '',
        tokens:
        {
            accessToken: '',
            refreshToken: ''
        },
        claims: [],
        userImageUrl: ''
    },
    setUser: () => console.log('no user'),
    logout: () => console.log("failed to log out"),
    isAuthenticated: () => { return false; },
    isUserLoaded: false
});

export const useUser = () => useContext(UserContext);