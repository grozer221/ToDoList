import {gql} from '@apollo/client';

export type LoginData = { login: string }

export type LoginVars = { loginAuthInputType: loginAuthInputType }
export type loginAuthInputType = {
    login: string,
    password: string,
}

export const LOGIN_MUTATION = gql`
    mutation Login($loginAuthInputType: LoginAuthInputType!) {
        login(loginAuthInputType: $loginAuthInputType) {
            user {
                ...UserFragment
            }
            token
        }
    }
`;
