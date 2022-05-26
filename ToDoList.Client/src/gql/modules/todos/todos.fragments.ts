import {gql} from "@apollo/client";

export const TODO_FRAGMENT = gql`
    fragment TodoFragment on ToDoType {
        id
        name
        isComplete
        deadline
        dateComplete
        categoryId
        createdAt
        updatedAt
    }
`