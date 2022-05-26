import {gql} from '@apollo/client';
import {Todo} from "./todos.types";
import {TODO_FRAGMENT} from "./todos.fragments";

export type TodosGetAllData = { toDos: { getAll: Todo[] } }
export type TodosGetAllVars = { like: string | null, sortOrder: string | null, categoryId: number | null }
export const TODOS_GET_ALL_QUERY = gql`
    ${TODO_FRAGMENT}
    query ToDosGetAll($like: String, $sortOrder: ToDosSortOrder, $categoryId: Int){
        toDos{
            getAll(like: $like, sortOrder: $sortOrder, categoryId: $categoryId){
                ...TodoFragment
            }
        }
    }
`;

export type TodosGetByIdData = { toDos: { getById: Todo } }
export type TodosGetByIdVars = { id: number }
export const TODOS_GET_BY_ID_QUERY = gql`
    ${TODO_FRAGMENT}
    query ToDosGetAll($id: Int!){
        toDos{
            getById(id: $id){
                ...TodoFragment
            }
        }
    }
`;
