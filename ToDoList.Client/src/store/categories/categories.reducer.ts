import {Reducer} from "redux";
import {Category} from "../../graphQL/modules/categories/categories.types";
import {CategoryActionTypes} from "./categories.actions";

const initialState = {
    pageSize: 0,
    total: 0,
    categories: [] as Category[],
    fetchCategoriesLoading: false,
    fetchCategoriesError: '',
    
    fetchCreateCategoryLoading: false,
    fetchCreateCategoryError: '',

    inUpdateCategory: null as Category | null,
    fetchInUpdateCategoryLoading: true,
    fetchInUpdateCategoryError: '',
    fetchUpdateCategoryLoading: false,
    fetchUpdateCategoryError: '',

    fetchRemoveCategoryLoading: false,
    fetchRemoveCategoryError: '',

    navigateTo: '',
}

export const categoriesReducer: Reducer<typeof initialState, CategoryActionTypes> = (state = initialState, action) => {
    switch (action.type) {
        case 'SET_PAGE_SIZE':
            return {...state, pageSize: action.payload};
        case 'SET_TOTAL':
            return {...state, total: action.payload};

        case 'SET_CATEGORIES':
            return {...state, categories: action.payload};
        case 'SET_FETCH_CATEGORIES_LOADING':
            return {...state, fetchCategoriesLoading: action.payload};
        case 'SET_FETCH_CATEGORIES_ERROR':
            return {...state, fetchCategoriesError: action.payload};

        case 'SET_FETCH_CREATE_CATEGORY_LOADING':
            return {...state, fetchCreateCategoryLoading: action.payload};
        case 'SET_FETCH_CREATE_CATEGORY_ERROR':
            return {...state, fetchCreateCategoryError: action.payload};


        case 'SET_IN_UPDATE_CATEGORY':
            return {...state, inUpdateCategory: action.payload};
        case 'SET_FETCH_IN_UPDATE_CATEGORY_LOADING':
            return {...state, fetchInUpdateCategoryLoading: action.payload};
        case 'SET_FETCH_IN_UPDATE_CATEGORY_ERROR':
            return {...state, fetchInUpdateCategoryError: action.payload};
        case 'SET_UPDATED_CATEGORY':
            return {...state, categories: state.categories.map(category => category.id == action.payload.id ? action.payload : category)};
        case 'SET_FETCH_UPDATE_CATEGORY_LOADING':
            return {...state, fetchUpdateCategoryLoading: action.payload};
        case 'SET_FETCH_UPDATE_CATEGORY_ERROR':
            return {...state, fetchUpdateCategoryError: action.payload};
        case 'RESET_AFTER_LEAVE_UPDATE_PAGE':
            return {
                ...state,
                inUpdateCategory: action.payload.inUpdateCategory,
                fetchInUpdateCategoryLoading: action.payload.fetchInUpdateCategoryLoading,
                fetchInUpdateCategoryError: action.payload.fetchInUpdateCategoryError,
                fetchUpdateCategoryLoading: action.payload.fetchUpdateCategoryLoading,
                fetchUpdateCategoryError: action.payload.fetchUpdateCategoryError,
            };

        case 'REMOVE_CATEGORY':
            return {...state, categories: state.categories.filter(category => category.id !== action.payload)};
        case 'SET_FETCH_REMOVE_CATEGORY_LOADING':
            return {...state, fetchRemoveCategoryLoading: action.payload};
        case 'SET_FETCH_REMOVE_CATEGORY_ERROR':
            return {...state, fetchRemoveCategoryError: action.payload};

        case 'SET_NAVIGATE_TO':
            return {...state, navigateTo: action.payload};
        default:
            return state;
    }
}