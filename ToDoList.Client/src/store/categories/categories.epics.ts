import {combineEpics, Epic, ofType} from "redux-observable";
import {RootState} from "../store";
import {catchError, endWith, from, map, mergeMap, of, startWith} from "rxjs";
import {categoriesActions} from "./categories.actions";
import {client} from "../../graphQL/client";
import {
    CATEGORIES_GET_ALL_QUERY,
    CATEGORIES_GET_BY_ID_QUERY,
    CategoriesGetAllData,
    CategoriesGetAllVars,
    CategoriesGetByIdData,
    CategoriesGetByIdVars
} from "../../graphQL/modules/categories/categories.queries";
import {
    CATEGORIES_CREATE_MUTATION,
    CATEGORIES_REMOVE_MUTATION,
    CATEGORIES_UPDATE_MUTATION,
    CategoriesCreateData,
    CategoriesCreateVars,
    CategoriesRemoveData,
    CategoriesRemoveVars,
    CategoriesUpdateData,
    CategoriesUpdateVars
} from "../../graphQL/modules/categories/categories.mutations";
import {Category} from "../../graphQL/modules/categories/categories.types";

export const fetchCategoriesEpic: Epic<ReturnType<typeof categoriesActions.fetchCategories>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_CATEGORIES'),
        mergeMap(action =>
            from(client.query<CategoriesGetAllData, CategoriesGetAllVars>({
                query: CATEGORIES_GET_ALL_QUERY,
                variables: {
                    page: action.payload.page,
                    like: action.payload.like,
                    sortOrder: action.payload.sortOrder,
                }
            })).pipe(
                mergeMap(response => [
                    categoriesActions.setCategories(response.data.categories.get.entities),
                    categoriesActions.setTotal(response.data.categories.get.total),
                    categoriesActions.setPageSize(response.data.categories.get.pageSize),
                ]),
                catchError(error => of(categoriesActions.setFetchCategoriesError(error))),
                startWith(categoriesActions.setFetchCategoriesLoading(true)),
                endWith(categoriesActions.setFetchCategoriesLoading(false)),
            )
        )
    );

export const fetchCreateCategoriesEpic: Epic<ReturnType<typeof categoriesActions.fetchCreateCategory>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_CREATE_CATEGORY'),
        mergeMap(action =>
            from(client.mutate<CategoriesCreateData, CategoriesCreateVars>({
                mutation: CATEGORIES_CREATE_MUTATION,
                variables: {categoriesCreateInputType: action.payload}
            })).pipe(
                mergeMap(response => [
                    categoriesActions.setCategories([response.data?.categories.create as Category, ...state$.value.categories.categories]),
                    categoriesActions.setNavigateTo('..'),
                ]),
                catchError(error => of(categoriesActions.setFetchCreateCategoryError(error))),
                startWith(categoriesActions.setFetchCreateCategoryLoading(true)),
                endWith(categoriesActions.setFetchCreateCategoryLoading(false)),
            )
        )
    );

export const fetchInUpdateCategoryEpic: Epic<ReturnType<typeof categoriesActions.fetchInUpdateCategory>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_IN_UPDATE_CATEGORY'),
        mergeMap(action =>
            from(client.query<CategoriesGetByIdData, CategoriesGetByIdVars>({
                query: CATEGORIES_GET_BY_ID_QUERY,
                variables: {id: action.payload}
            })).pipe(
                map(response => categoriesActions.setInUpdateCategory(response.data.categories.getById)),
                catchError(error => of(categoriesActions.setFetchCategoriesError(error))),
                startWith(categoriesActions.setFetchInUpdateCategoryLoading(true)),
                endWith(categoriesActions.setFetchInUpdateCategoryLoading(false)),
            )
        ),
    );

export const fetchUpdateCategoryEpic: Epic<ReturnType<typeof categoriesActions.fetchUpdateCategory>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_UPDATE_CATEGORY'),
        mergeMap(action =>
            from(client.mutate<CategoriesUpdateData, CategoriesUpdateVars>({
                mutation: CATEGORIES_UPDATE_MUTATION,
                variables: {categoriesUpdateInputType: action.payload}
            })).pipe(
                mergeMap(response => [
                    categoriesActions.setUpdatedCategory(response.data?.categories.update as Category),
                    categoriesActions.setNavigateTo('..')
                ]),
                catchError(error => of(categoriesActions.setFetchCategoriesError(error))),
                startWith(categoriesActions.setFetchUpdateCategoryLoading(true)),
                endWith(categoriesActions.setFetchUpdateCategoryLoading(false)),
            )
        )
    );

export const fetchRemoveCategoryEpic: Epic<ReturnType<typeof categoriesActions.fetchRemoveCategory>, any, RootState> = (action$, state$) =>
    action$.pipe(
        ofType('FETCH_REMOVE_CATEGORY'),
        mergeMap(action =>
            from(client.mutate<CategoriesRemoveData, CategoriesRemoveVars>({
                mutation: CATEGORIES_REMOVE_MUTATION,
                variables: {id: action.payload}
            })).pipe(
                map(response => categoriesActions.removeCategory(response.data?.categories.remove.id as number)),
                catchError(error => of(categoriesActions.setFetchRemoveCategoryError(error))),
                startWith(categoriesActions.setFetchRemoveCategoryLoading(true)),
                endWith(categoriesActions.setFetchRemoveCategoryLoading(false)),
            )
        )
    );

// @ts-ignore
export const categoriesEpics = combineEpics(fetchCategoriesEpic, fetchCreateCategoriesEpic, fetchInUpdateCategoryEpic, fetchUpdateCategoryEpic, fetchRemoveCategoryEpic)