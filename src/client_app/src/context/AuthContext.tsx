'use client';
import { createContext } from "react";

interface AuthContextType {
    isAuthenticated: boolean|undefined;
    accessToken: string | undefined;
}

export const AuthContext = createContext<AuthContextType>({} as AuthContextType);

interface AuthProviderProps {
    children: React.ReactNode,
    initialAuthState: AuthContextType
}

export const AuthProvider = ({children, initialAuthState}: AuthProviderProps) => {
    return (
        <AuthContext.Provider value={{isAuthenticated: false, accessToken: ""}}>
            {children}
        </AuthContext.Provider>
    )
}




