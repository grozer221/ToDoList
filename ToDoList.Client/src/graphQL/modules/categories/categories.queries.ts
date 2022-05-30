import {gql} from '@apollo/client';
import {CATEGORY_FRAGMENT} from "./categories.fragments";
import {Category} from "./categories.types";
import {CategoriesSortOrder} from "../../enums/categoriesSortOrder";

export type CategoriesGetAllData = { categories: { getAll: Category[] } }
export type CategoriesGetAllVars = { like: string | null, sortOrder: CategoriesSortOrder | null }
export const CATEGORIES_GET_ALL_QUERY = gql`
    ${CATEGORY_FRAGMENT}
    query CategoriesGetAll($like: String, $sortOrder: CategoriesSortOrder){
        categories{
            getAll(like: $like, sortOrder: $sortOrder){
                ...CategoryFragment
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
