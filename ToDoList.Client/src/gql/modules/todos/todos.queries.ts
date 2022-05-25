import {gql} from '@apollo/client';
import {Todo} from "./todos.types";

export type TodosGetAllData = { toDos: { getAll: Todo[] } }
export type TodosGetAllVars = { like: string, sortOrder: string, categoryId: number }

export const TODOS_GET_ALL_QUERY = gql`
    query ToDosGetAll($like: String, $sortOrder: ToDosSortOrder, $categoryId: Int){
        toDos{
            getAll(like: $like, sortOrder: $sortOrder, categoryId: $categoryId){
                id
                name
                isComplete
                deadline
                dateComplete
                categoryId
                category{
                    name
                }
                createdAt
                updatedAt
            }
        }
    }
`;
