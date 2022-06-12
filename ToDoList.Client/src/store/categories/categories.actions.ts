import {Category} from "../../graphQL/modules/categories/categories.types";
import {ValueOf} from "../store";
import {CategoriesCreateInputType, CategoriesUpdateInputType} from "../../graphQL/modules/categories/categories.mutations";
import {CategoriesSortOrder} from "../../graphQL/enums/categoriesSortOrder";

export const categoriesActions = {
    setPageSize: (pageSize: number) => ({
        type: 'SET_PAGE_SIZE',
        payload: pageSize,
    } as const),
    setTotal: (total: number) => ({
        type: 'SET_TOTAL',
        payload: total,
    } as const),

    fetchCategories: (page: number, like: string | null, sortOrder: CategoriesSortOrder | null) => ({
        type: 'FETCH_CATEGORIES',
        payload: {page, like, sortOrder},
    } as const),
    setCategories: (todos: Category[]) => ({
        type: 'SET_CATEGORIES',
        payload: todos,
    } as const),
    setFetchCategoriesLoading: (loading: boolean) => ({
        type: 'SET_FETCH_CATEGORIES_LOADING',
        payload: loading,
    } as const),
    setFetchCategoriesError: (error: string) => ({
        type: 'SET_FETCH_CATEGORIES_ERROR',
        payload: error,
    } as const),


    fetchCreateCategory: (toDosCreateInputType: CategoriesCreateInputType) => ({
        type: 'FETCH_CREATE_CATEGORY',
        payload: toDosCreateInputType,
    } as const),
    setFetchCreateCategoryLoading: (loading: boolean) => ({
        type: 'SET_FETCH_CREATE_CATEGORY_LOADING',
        payload: loading,
    } as const),
    setFetchCreateCategoryError: (error: string) => ({
        type: 'SET_FETCH_CREATE_CATEGORY_ERROR',
        payload: error,
    } as const),

    fetchInUpdateCategory: (id: number) => ({
        type: 'FETCH_IN_UPDATE_CATEGORY',
        payload: id,
    } as const),
    setInUpdateCategory: (todo: Category | null) => ({
        type: 'SET_IN_UPDATE_CATEGORY', payload: todo
    } as const),
    setFetchInUpdateCategoryLoading: (loading: boolean) => ({
        type: 'SET_FETCH_IN_UPDATE_CATEGORY_LOADING',
        payload: loading,
    } as const),
    setFetchInUpdateCategoryError: (error: string) => ({
        type: 'SET_FETCH_IN_UPDATE_CATEGORY_ERROR',
        payload: error,
    } as const),
    fetchUpdateCategory: (toDosUpdateInputType: CategoriesUpdateInputType) => ({
        type: 'FETCH_UPDATE_CATEGORY',
        payload: toDosUpdateInputType,
    } as const),
    setUpdatedCategory: (todo: Category) => ({
        type: 'SET_UPDATED_CATEGORY',
        payload: todo,
    } as const),
    setFetchUpdateCategoryLoading: (loading: boolean) => ({
        type: 'SET_FETCH_UPDATE_CATEGORY_LOADING',
        payload: loading,
    } as const),
    setFetchUpdateCategoryError: (error: string) => ({
        type: 'SET_FETCH_UPDATE_CATEGORY_ERROR',
        payload: error,
    } as const),
    resetAfterLeaveUpdatePage: () => ({
        type: 'RESET_AFTER_LEAVE_UPDATE_PAGE',
        payload: {
            inUpdateCategory: null,
            fetchInUpdateCategoryLoading: true,
            fetchInUpdateCategoryError: '',
            fetchUpdateCategoryLoading: false,
            fetchUpdateCategoryError: '',
        },
    } as const),

    fetchRemoveCategory: (id: number) => ({
        type: 'FETCH_REMOVE_CATEGORY',
        payload: id,
    } as const),
    removeCategory: (id: number) => ({
        type: 'REMOVE_CATEGORY',
        payload: id,
    } as const),
    setFetchRemoveCategoryLoading: (loading: boolean) => ({
        type: 'SET_FETCH_REMOVE_CATEGORY_LOADING',
        payload: loading,
    } as const),
    setFetchRemoveCategoryError: (error: string) => ({
        type: 'SET_FETCH_REMOVE_CATEGORY_ERROR',
        payload: error,
    } as const),

    setNavigateTo: (route: string) => ({
        type: 'SET_NAVIGATE_TO',
        payload: route,
    } as const),
};

export type CategoryActionCreatorTypes = ValueOf<typeof categoriesActions>;
export type CategoryActionTypes = ReturnType<CategoryActionCreatorTypes>;
