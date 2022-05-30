import { applyMiddleware, combineReducers, createStore } from 'redux';
import {composeWithDevTools} from "redux-devtools-extension";
import {todosReducer} from "./todos/todos.reducer";
import {combineEpics, createEpicMiddleware} from "redux-observable";
import {todosEpics} from "./todos/todos.epics";
import {categoriesReducer} from "./categories/categories.reducer";
import {categoriesEpics} from "./categories/categories.epics";

const epicMiddleware = createEpicMiddleware();

export const store = createStore(combineReducers({
    todos: todosReducer,
    categories: categoriesReducer,
}), composeWithDevTools(applyMiddleware(epicMiddleware)));

// @ts-ignore
const rootEpic = combineEpics(todosEpics, categoriesEpics);
// @ts-ignore
epicMiddleware.run(rootEpic);

export type ValueOf<T> = T[keyof T]
export type RootState = ReturnType<typeof store.getState>;