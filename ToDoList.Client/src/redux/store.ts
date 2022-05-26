import { applyMiddleware, combineReducers, createStore } from 'redux';
import {composeWithDevTools} from "redux-devtools-extension";
import {todosReducer} from "./todos/todos.reducer";
import {combineEpics, createEpicMiddleware} from "redux-observable";
import {todosEpics} from "./todos/todos.epics";

const epicMiddleware = createEpicMiddleware();

export const store = createStore(combineReducers({
    todos: todosReducer,
}), composeWithDevTools(applyMiddleware(epicMiddleware)));

const rootEpic = combineEpics(todosEpics);
// @ts-ignore
epicMiddleware.run(rootEpic);

export type ValueOf<T> = T[keyof T]
export type RootState = ReturnType<typeof store.getState>;