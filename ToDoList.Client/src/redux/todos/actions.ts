import {Todo} from "../../gql/modules/todos/todos.types";

export type GetTodosAction = { type: 'GET_TODOS', payload: {like: string, sortOrder: string, categoryId: number} }
export type SetTodosAction = { type: 'SET_TODOS', payload: Todo[] }
export type AddTodoAction = { type: 'ADD_TODO', payload: Todo }
export type SetFetchTodosErrorAction = { type: 'SET_FETCH_TODOS_ERROR', payload: string }

export const todosActions = {
    getTodos: () => ({type: 'GET_TODOS', payload: {}} as GetTodosAction),
    setTodos: (todos: Todo[]) => ({type: 'SET_TODOS', payload: todos} as SetTodosAction),
    addTodo: (todo: Todo) => ({type: 'ADD_TODO', payload: todo} as AddTodoAction),
    setFetchTodosError: (error: string) => ({type: 'SET_FETCH_TODOS_ERROR', payload: error} as SetFetchTodosErrorAction),
};

export type ActionCreators = GetTodosAction | SetTodosAction | AddTodoAction | SetFetchTodosErrorAction;