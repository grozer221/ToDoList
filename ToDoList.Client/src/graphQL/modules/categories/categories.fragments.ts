import {gql} from "@apollo/client";

export const CATEGORY_FRAGMENT = gql`
    fragment CategoryFragment on CategoryType {
        id
        name
        createdAt
        updatedAt
    }
`