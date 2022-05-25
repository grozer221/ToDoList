import { applyMiddleware, combineReducers, createStore } from 'redux';
import {composeWithDevTools} from "redux-devtools-extension";
import {todosReducer} from "./todos/reducer";
import {combineEpics, createEpicMiddleware} from "redux-observable";
import {getTodosEpic} from "./todos/epics";

const epicMiddleware = createEpicMiddleware();

export const store = createStore(combineReducers({
    todos: todosReducer,
}), composeWithDevTools(applyMiddleware(epicMiddleware)));

// @ts-ignore
const rootEpic = combineEpics(getTodosEpic);
// @ts-ignore
epicMiddleware.run(rootEpic);

export type RootState = ReturnType<typeof store.getState>;
