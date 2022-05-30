import {gql} from "@apollo/client";
import {CATEGORY_FRAGMENT} from "../categories/categories.fragments";

export const TODO_FRAGMENT = gql`
    ${CATEGORY_FRAGMENT}
    fragment TodoFragment on ToDoType {
        id
        name
        isComplete
        deadline
        dateComplete
        categoryId
        category @include(if: $withCategory) {
            ...CategoryFragment
        }
        createdAt
        updatedAt
    }
`