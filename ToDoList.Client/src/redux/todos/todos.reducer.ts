import {Reducer} from "redux";
import {Todo} from "../../gql/modules/todos/todos.types";
import {TodoActionTypes} from "./todos.actions";

const initialState = {
    todos: [] as Todo[],
    fetchTodosLoading: false,
    fetchTodosError: '',

    fetchCreateTodoLoading: false,
    fetchCreateTodoError: '',

    inUpdateTodo: null as Todo | null,
    fetchInUpdateTodoLoading: true,
    fetchInUpdateTodoError: '',
    fetchUpdateTodoLoading: false,
    fetchUpdateTodoError: '',

    fetchRemoveTodoLoading: false,
    fetchRemoveTodoError: '',
}

type InitialState = typeof initialState

export const todosReducer: Reducer<InitialState, TodoActionTypes> = (state = initialState, action): InitialState => {
    switch (action.type) {
        case 'SET_TODOS':
            return {...state, todos: action.payload};
        case 'SET_FETCH_TODOS_LOADING':
            return {...state, fetchTodosLoading: action.payload};
        case 'SET_FETCH_TODOS_ERROR':
            return {...state, fetchTodosError: action.payload};

        case 'SET_FETCH_CREATE_TODO_LOADING':
            return {...state, fetchCreateTodoLoading: action.payload};
        case 'SET_FETCH_CREATE_TODO_ERROR':
            return {...state, fetchCreateTodoError: action.payload};


        case 'SET_IN_UPDATE_TODO':
            return {...state, inUpdateTodo: action.payload};
        case 'SET_FETCH_IN_UPDATE_TODO_LOADING':
            return {...state, fetchInUpdateTodoLoading: action.payload};
        case 'SET_FETCH_IN_UPDATE_TODO_ERROR':
            return {...state, fetchInUpdateTodoError: action.payload};
        case 'SET_UPDATED_TODO':
            return {...state, todos: state.todos.map(todo => todo.id == action.payload.id ? action.payload : todo)};
        case 'SET_FETCH_UPDATE_TODO_LOADING':
            return {...state, fetchUpdateTodoLoading: action.payload};
        case 'SET_FETCH_UPDATE_TODO_ERROR':
            return {...state, fetchUpdateTodoError: action.payload};
        case 'RESET_AFTER_LEAVE_UPDATE_PAGE':
            return {
                ...state,
                inUpdateTodo: action.payload.inUpdateTodo,
                fetchInUpdateTodoLoading: action.payload.fetchInUpdateTodoLoading,
                fetchInUpdateTodoError: action.payload.fetchInUpdateTodoError,
                fetchUpdateTodoLoading: action.payload.fetchUpdateTodoLoading,
                fetchUpdateTodoError: action.payload.fetchUpdateTodoError,
            };

        case 'REMOVE_TODO':
            return {...state, todos: state.todos.filter(todo => todo.id !== action.payload)};
        case 'SET_FETCH_REMOVE_TODO_LOADING':
            return {...state, fetchRemoveTodoLoading: action.payload};
        case 'SET_FETCH_REMOVE_TODO_ERROR':
            return {...state, fetchRemoveTodoError: action.payload};
        default:
            return state;
    }
}