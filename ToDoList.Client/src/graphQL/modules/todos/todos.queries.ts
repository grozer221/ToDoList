import {gql} from '@apollo/client';
import {Todo} from "./todos.types";
import {TODO_FRAGMENT} from "./todos.fragments";
import {TodosSortOrder} from "../../enums/todosSortOrder";

export type TodosGetAllData = { toDos: { getAll: Todo[] } }
export type TodosGetAllVars = { like: string | null, sortOrder: TodosSortOrder | null, categoryId: number | null, withCategory: boolean }
export const TODOS_GET_ALL_QUERY = gql`
    ${TODO_FRAGMENT}
    query ToDosGetAll($like: String, $sortOrder: ToDosSortOrder, $categoryId: Int, $withCategory: Boolean!){
        toDos{
            getAll(like: $like, sortOrder: $sortOrder, categoryId: $categoryId){
                ...TodoFragment
            }
        }
    }
`;

export type TodosGetByIdData = { toDos: { getById: Todo } }
export type TodosGetByIdVars = { id: number, withCategory: boolean }
export const TODOS_GET_BY_ID_QUERY = gql`
    ${TODO_FRAGMENT}
    query ToDosGetAll($id: Int!, $withCategory: Boolean!){
        toDos{
            getById(id: $id){
                ...TodoFragment
            }
        }
    }
`;
