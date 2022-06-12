import {gql} from '@apollo/client';
import {CATEGORY_FRAGMENT} from "./categories.fragments";
import {Category} from "./categories.types";
import {CategoriesSortOrder} from "../../enums/categoriesSortOrder";
import {GetEntitiesResponse} from "../../types/getEntitiesResponse";

export type CategoriesGetAllData = { categories: { get: GetEntitiesResponse<Category> } }
export type CategoriesGetAllVars = { page: number, like: string | null, sortOrder: CategoriesSortOrder | null }
export const CATEGORIES_GET_ALL_QUERY = gql`
    ${CATEGORY_FRAGMENT}
    query CategoriesGetAll($page: Int!, $like: String, $sortOrder: CategoriesSortOrder){
        categories{
            get(page: $page, like: $like, sortOrder: $sortOrder){
                entities {
                    ...CategoryFragment
                }
                pageSize
                total
            }
        }
    }
`;

export type CategoriesGetByIdData = { categories: { getById: Category } }
export type CategoriesGetByIdVars = { id: number }
export const CATEGORIES_GET_BY_ID_QUERY = gql`
    ${CATEGORY_FRAGMENT}
    query CategoriesGetAll($id: Int!){
        categories{
            getById(id: $id){
                ...CategoryFragment
            }
        }
    }
`;
