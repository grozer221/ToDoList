import {Todo} from "../../gql/modules/todos/todos.types";
import {ValueOf} from "../store";
import {TodosCreateInputType, TodosCreateVars, ToDosUpdateInputType} from "../../gql/modules/todos/todos.mutations";

export const todosActions = {
    fetchTodos: (like: string | null, sortOrder: string | null, categoryId: number | null) => ({
        type: 'FETCH_TODOS',
        payload: {like, sortOrder, categoryId},
    } as const),
    setTodos: (todos: Todo[]) => ({
        type: 'SET_TODOS',
        payload: todos,
    } as const),
    setFetchTodosLoading: (loading: boolean) => ({
        type: 'SET_FETCH_TODOS_LOADING',
        payload: loading,
    } as const),
    setFetchTodosError: (error: string) => ({
        type: 'SET_FETCH_TODOS_ERROR',
        payload: error,
    } as const),


    fetchCreateTodo: (toDosCreateInputType: TodosCreateInputType) => ({
        type: 'FETCH_CREATE_TODO',
        payload: toDosCreateInputType,
    } as const),
    setFetchCreateTodoLoading: (loading: boolean) => ({
        type: 'SET_FETCH_CREATE_TODO_LOADING',
        payload: loading,
    } as const),
    setFetchCreateTodoError: (error: string) => ({
        type: 'SET_FETCH_CREATE_TODO_ERROR',
        payload: error,
    } as const),

    fetchInUpdateTodo: (id: number) => ({
        type: 'FETCH_IN_UPDATE_TODO',
        payload: id,
    } as const),
    setInUpdateTodo: (todo: Todo | null) => ({
        type: 'SET_IN_UPDATE_TODO', payload: todo
    } as const),
    setFetchInUpdateTodoLoading: (loading: boolean) => ({
        type: 'SET_FETCH_IN_UPDATE_TODO_LOADING',
        payload: loading,
    } as const),
    setFetchInUpdateTodoError: (error: string) => ({
        type: 'SET_FETCH_IN_UPDATE_TODO_ERROR',
        payload: error,
    } as const),
    fetchUpdateTodo: (toDosUpdateInputType: ToDosUpdateInputType) => ({
        type: 'FETCH_UPDATE_TODO',
        payload: toDosUpdateInputType,
    } as const),
    setUpdatedTodo: (todo: Todo) => ({
        type: 'SET_UPDATED_TODO',
        payload: todo,
    } as const),
    setFetchUpdateTodoLoading: (loading: boolean) => ({
        type: 'SET_FETCH_UPDATE_TODO_LOADING',
        payload: loading,
    } as const),
    setFetchUpdateTodoError: (error: string) => ({
        type: 'SET_FETCH_UPDATE_TODO_ERROR',
        payload: error,
    } as const),
    resetAfterLeaveUpdatePage: () => ({
        type: 'RESET_AFTER_LEAVE_UPDATE_PAGE',
        payload: {
            inUpdateTodo: null,
            fetchInUpdateTodoLoading: true,
            fetchInUpdateTodoError: '',
            fetchUpdateTodoLoading: false,
            fetchUpdateTodoError: '',
        },
    } as const),

    fetchRemoveTodo: (id: number) => ({
        type: 'FETCH_REMOVE_TODO',
        payload: id,
    } as const),
    removeTodo: (id: number) => ({
        type: 'REMOVE_TODO',
        payload: id,
    } as const),
    setFetchRemoveTodoLoading: (loading: boolean) => ({
        type: 'SET_FETCH_REMOVE_TODO_LOADING',
        payload: loading,
    } as const),
    setFetchRemoveTodoError: (error: string) => ({
        type: 'SET_FETCH_REMOVE_TODO_ERROR',
        payload: error,
    } as const),
};

export type TodoActionCreatorTypes = ValueOf<typeof todosActions>;
export type TodoActionTypes = ReturnType<TodoActionCreatorTypes>;
