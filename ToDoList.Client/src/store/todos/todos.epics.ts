import {combineEpics, Epic, ofType} from "redux-observable";
import {RootState} from "../store";
import {catchError, endWith, from, map, mergeMap, of, startWith} from "rxjs";
import {todosActions} from "./todos.actions";
import {client} from "../../graphQL/client";
import {
    TODOS_GET_ALL_QUERY,
    TODOS_GET_BY_ID_QUERY,
    TodosGetAllData,
    TodosGetAllVars,
    TodosGetByIdData,
    TodosGetByIdVars
} from "../../graphQL/modules/todos/todos.queries";
import {
    TODOS_CREATE_MUTATION,
    TODOS_REMOVE_MUTATION,
    TODOS_UPDATE_MUTATION,
    TodosCreateData,
    TodosCreateVars,
    TodosRemoveData,
    TodosRemoveVars,
    TodosUpdateData,
    TodosUpdateVars
} from "../../graphQL/modules/todos/todos.mutations";
import {Todo} from "../../graphQL/modules/todos/todos.types";

export const fetchTodosEpic: Epic<ReturnType<typeof todosActions.fetchTodos>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_TODOS'),
        mergeMap(action =>
            from(client.query<TodosGetAllData, TodosGetAllVars>({
                query: TODOS_GET_ALL_QUERY,
                variables: {
                    page: action.payload.page,
                    like: action.payload.like,
                    sortOrder: action.payload.sortOrder,
                    categoryId: action.payload.categoryId,
                    withCategory: true,
                }
            })).pipe(
                mergeMap(response => [
                    todosActions.setTodos(response.data.toDos.get.entities),
                    todosActions.setTotal(response.data.toDos.get.total),
                    todosActions.setPageSize(response.data.toDos.get.pageSize),
                ]),
                catchError(error => of(todosActions.setFetchTodosError(error))),
                startWith(todosActions.setFetchTodosLoading(true)),
                endWith(todosActions.setFetchTodosLoading(false)),
            )
        )
    );

export const fetchCreateTodosEpic: Epic<ReturnType<typeof todosActions.fetchCreateTodo>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_CREATE_TODO'),
        mergeMap(action =>
            from(client.mutate<TodosCreateData, TodosCreateVars>({
                mutation: TODOS_CREATE_MUTATION,
                variables: {toDosCreateInputType: action.payload, withCategory: true}
            })).pipe(
                map(response => todosActions.setTodos([response.data?.toDos.create as Todo, ...state$.value.todos.todos])),
                catchError(error => of(todosActions.setFetchCreateTodoError(error))),
                startWith(todosActions.setFetchCreateTodoLoading(true)),
                endWith(todosActions.setFetchCreateTodoLoading(false)),
            )
        )
    );

export const fetchInUpdateTodoEpic: Epic<ReturnType<typeof todosActions.fetchInUpdateTodo>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_IN_UPDATE_TODO'),
        mergeMap(action =>
            from(client.query<TodosGetByIdData, TodosGetByIdVars>({
                query: TODOS_GET_BY_ID_QUERY,
                variables: {id: action.payload, withCategory: true}
            })).pipe(
                map(response => todosActions.setInUpdateTodo(response.data.toDos.getById)),
                catchError(error => of(todosActions.setFetchTodosError(error))),
                startWith(todosActions.setFetchInUpdateTodoLoading(true)),
                endWith(todosActions.setFetchInUpdateTodoLoading(false)),
            )
        ),
    );

export const fetchUpdateTodoEpic: Epic<ReturnType<typeof todosActions.fetchUpdateTodo>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_UPDATE_TODO'),
        mergeMap(action =>
            from(client.mutate<TodosUpdateData, TodosUpdateVars>({
                mutation: TODOS_UPDATE_MUTATION,
                variables: {toDosUpdateInputType: action.payload, withCategory: true}
            })).pipe(
                mergeMap(response => [
                    todosActions.setUpdatedTodo(response.data?.toDos.update as Todo),
                    todosActions.setNavigateTo('..')
                ]),
                catchError(error => of(todosActions.setFetchTodosError(error))),
                startWith(todosActions.setFetchUpdateTodoLoading(true)),
                endWith(todosActions.setFetchUpdateTodoLoading(false)),
            )
        )
    )
;

export const fetchRemoveTodoEpic: Epic<ReturnType<typeof todosActions.fetchRemoveTodo>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_REMOVE_TODO'),
        mergeMap(action =>
            from(client.mutate<TodosRemoveData, TodosRemoveVars>({
                mutation: TODOS_REMOVE_MUTATION,
                variables: {id: action.payload, withCategory: true}
            })).pipe(
                map(response => todosActions.removeTodo(response.data?.toDos.remove.id as number)),
                catchError(error => of(todosActions.setFetchRemoveTodoError(error))),
                startWith(todosActions.setFetchRemoveTodoLoading(true)),
                endWith(todosActions.setFetchRemoveTodoLoading(false)),
            )
        )
    );

// @ts-ignore
export const todosEpics = combineEpics(fetchTodosEpic, fetchCreateTodosEpic, fetchInUpdateTodoEpic, fetchUpdateTodoEpic, fetchRemoveTodoEpic)