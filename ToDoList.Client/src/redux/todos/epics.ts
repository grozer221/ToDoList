import {Epic, ofType} from "redux-observable";
import {RootState} from "../store";
import {catchError, from, map, mergeMap, of} from "rxjs";
import {GetTodosAction, todosActions} from "./actions";
import {client} from "../../gql/client";
import {TODOS_GET_ALL_QUERY, TodosGetAllData, TodosGetAllVars} from "../../gql/modules/todos/todos.queries";

export const getTodosEpic: Epic<GetTodosAction, any, RootState> = (action$, store) =>
    action$.pipe(
        ofType(todosActions.getTodos().type),
        mergeMap((action) =>
            from(client.query<TodosGetAllData, TodosGetAllVars>({
                query: TODOS_GET_ALL_QUERY,
                variables: {
                    like: action.payload.like,
                    sortOrder: action.payload.sortOrder,
                    categoryId: action.payload.categoryId,
                }
            })).pipe(
                map(response => todosActions.setTodos(response.data.toDos.getAll)),
                catchError((error) => of(todosActions.setFetchTodosError(error)))
            )
        )
    );