import { useEffect, useState } from "react";
import { getProfilePictureUrl, getUserID, getUserTokens, removeProfilePictureUrl, removeUserID, removeUserTokens } from "../../services/LocalStorageFunctions";
import { UserContext } from ".";
import { User } from "./UserTypes";
import jwtDecode from "jwt-decode";
import { getUserClaims } from "../../resources";

const initialUser: User = {
    id: '',
    tokens:
    {
        accessToken: '',
        refreshToken: ''
    },
    claims: [],
    userImageUrl: ''
}

export default function UserProvider(props: any) {
    const [user, setUser] = useState<User>(initialUser);
    const [isUserLoaded, setIsUserLoaded] = useState<boolean>(false);
    //Use effect for initial load of user tokens from local storage
    useEffect(() => {
        async function initUser() {
            setIsUserLoaded(false);
            const id = getUserID();
            const tokensFromLocal = getUserTokens();
            const profilePictureUrl = getProfilePictureUrl();

            if(tokensFromLocal.refreshToken == ""){
                setIsUserLoaded(true);
                return
            }

            try {
                const claims = await getUserClaims();

                const user: User = {
                    id: id,
                    tokens: tokensFromLocal,
                    claims: claims.data,
                    userImageUrl: profilePictureUrl
                }
                setUser(user);

            } catch (err) {
                logout();
            }
            finally {
                setIsUserLoaded(true);
            }
        }

        initUser();
    }, [])

    //logout function that resets the user state and clears local storage
    function logout() {
        setUser(initialUser);
        removeUserTokens();
        removeProfilePictureUrl();
        removeUserID();
    }

    function isAuthenticated() {
        const { accessToken } = user?.tokens || {};

        if (!accessToken) {
            return false;
        }

        // Check if the access token has expired
        const decodedToken: { exp: number } = jwtDecode(accessToken);

        return decodedToken != undefined;
        // const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds

        // return decodedToken.exp > currentTime;
    }

    return (
        <UserContext.Provider value={{
            user: user, setUser: setUser, logout: logout, isAuthenticated: isAuthenticated, isUserLoaded: isUserLoaded
        }}>
            {props.children}
        </UserContext.Provider>
    )
}