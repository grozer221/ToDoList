import {gql} from '@apollo/client';
import {Todo} from "./todos.types";
import {TODO_FRAGMENT} from "./todos.fragments";

export type TodosUpdateData = { toDos: { update: Todo } }
export type TodosUpdateVars = { toDosUpdateInputType: ToDosUpdateInputType }
export type ToDosUpdateInputType = {
    id: number,
    name: string,
    deadline: string,
    categoryId: number | null,
    isComplete: boolean,
}
export const TODOS_UPDATE_MUTATION = gql`
    ${TODO_FRAGMENT}
    mutation ToDosUpdate($toDosUpdateInputType: ToDosUpdateInputType!){
        toDos{
            update(toDosUpdateInputType: $toDosUpdateInputType){
                ...TodoFragment
            }
        }
    }
`;

export type TodosCreateData = { toDos: { create: Todo } }
export type TodosCreateVars = { toDosCreateInputType: TodosCreateInputType }
export type TodosCreateInputType = {
    name: string,
    deadline: string,
    categoryId: number | null,
}
export const TODOS_CREATE_MUTATION = gql`
    ${TODO_FRAGMENT}
    mutation ToDosUpdate($toDosCreateInputType: ToDosCreateInputType!){
        toDos{
            create(toDosCreateInputType: $toDosCreateInputType){
                ...TodoFragment
            }
        }
    }
`;

export type TodosRemoveData = { toDos: { remove: Todo } }
export type TodosRemoveVars = { id: number }
export const TODOS_REMOVE_MUTATION = gql`
    ${TODO_FRAGMENT}
    mutation ToDosRemove($id: Int!){
        toDos{
            remove(id: $id){
                ...TodoFragment
            }
        }
    }
`;