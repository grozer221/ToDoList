import {gql} from '@apollo/client';
import {Category} from "./categories.types";
import {CATEGORY_FRAGMENT} from "./categories.fragments";

export type CategoriesCreateData = { categories: { create: Category } }
export type CategoriesCreateVars = { categoriesCreateInputType: CategoriesCreateInputType }
export type CategoriesCreateInputType = {
    name: string,
}
export const CATEGORIES_CREATE_MUTATION = gql`
    ${CATEGORY_FRAGMENT}
    mutation CategoriesCreate($categoriesCreateInputType: CategoriesCreateInputType!){
        categories{
            create(categoriesCreateInputType: $categoriesCreateInputType){
                ...CategoryFragment
            }
        }
    }
`;

export type CategoriesUpdateData = { categories: { update: Category } }
export type CategoriesUpdateVars = { categoriesUpdateInputType: CategoriesUpdateInputType }
export type CategoriesUpdateInputType = {
    id: number,
    name: string,
}
export const CATEGORIES_UPDATE_MUTATION = gql`
    ${CATEGORY_FRAGMENT}
    mutation CategoriesUpdate($categoriesUpdateInputType: CategoriesUpdateInputType!){
        categories{
            update(categoriesUpdateInputType: $categoriesUpdateInputType){
                ...CategoryFragment
            }
        }
    }
`;

export type CategoriesRemoveData = { categories: { remove: Category } }
export type CategoriesRemoveVars = { id: number }
export const CATEGORIES_REMOVE_MUTATION = gql`
    ${CATEGORY_FRAGMENT}
    mutation CategoriesRemove($id: Int!){
        categories{
            remove(id: $id){
                ...CategoryFragment
            }
        }
    }
`;