import {gql} from '@apollo/client';
import {Todo} from "./todos.types";
import {TODO_FRAGMENT} from "./todos.fragments";
import {TodosSortOrder} from "../../enums/todosSortOrder";
import {GetEntitiesResponse} from "../../types/getEntitiesResponse";

export type TodosGetAllData = { toDos: { get: GetEntitiesResponse<Todo> } }
export type TodosGetAllVars = { page: number, like: string | null, sortOrder: TodosSortOrder | null, categoryId: number | null, withCategory: boolean }
export const TODOS_GET_ALL_QUERY = gql`
    ${TODO_FRAGMENT}
    query ToDosGetAll($page: Int!, $like: String, $sortOrder: ToDosSortOrder, $categoryId: Int, $withCategory: Boolean!){
        toDos{
            get(page: $page, like: $like, sortOrder: $sortOrder, categoryId: $categoryId){
                entities {
                    ...TodoFragment
                }
                pageSize
                total
            }
        }
    }
`;

export type TodosGetByIdData = { toDos: { getById: Todo } }
export type TodosGetByIdVars = { id: number, withCategory: boolean }
export const TODOS_GET_BY_ID_QUERY = gql`
    ${TODO_FRAGMENT}
    query ToDosGetById($id: Int!, $withCategory: Boolean!){
        toDos{
            getById(id: $id){
                ...TodoFragment
            }
        }
    }
`;
