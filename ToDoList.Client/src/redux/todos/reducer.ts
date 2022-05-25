import {Reducer} from "redux";
import {ActionCreators} from "./actions";
import {Todo} from "../../gql/modules/todos/todos.types";

const initialState = {
    todos: [] as Todo[],
    fetchTodosError: '',
}

type InitialState = typeof initialState

export const todosReducer: Reducer<InitialState, ActionCreators> = (state = initialState, action): InitialState => {
    switch (action.type) {
        case 'ADD_TODO':
            return {...state, todos: [...state.todos, action.payload]};
        case 'SET_FETCH_TODOS_ERROR':
            return {...state, fetchTodosError: action.payload};
        case 'SET_TODOS':
            return {...state, todos: action.payload};
        default:
            return state;
    }
}