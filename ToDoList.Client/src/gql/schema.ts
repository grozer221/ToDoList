import {gql} from '@apollo/client';

export const schema = gql`
  schema {
  query: RootQueries
  mutation: RootMutations
}

type RootQueries {
  toDos: ToDosQueries
  categories: CategoriesQueries
}

type ToDosQueries {
  getAll(
    """
    Argument for Get ToDos
    """
    like: String

    """
    Argument for Get ToDos
    """
    sortOrder: ToDosSortOrder

    """
    Argument for Get ToDos
    """
    categoryId: Int
  ): [ToDoType]!
  getById(
    """
    Argument for Get ToDo
    """
    id: Int! = 0
  ): ToDoType!
}

type ToDoType {
  id: Int!
  name: String!
  isComplete: Boolean!
  deadline: DateTime
  dateComplete: DateTime
  categoryId: Int
  category: CategoryType
  createdAt: DateTime!
  updatedAt: DateTime!
}

"""
The \`DateTime\` scalar type represents a date and time. \`DateTime\` expects timestamps to be formatted in accordance with the [ISO-8601](https://en.wikipedia.org/wiki/ISO_8601) standard.
"""
scalar DateTime

type CategoryType {
  id: Int!
  name: String!
  toDos: [ToDoType]!
  createdAt: DateTime!
  updatedAt: DateTime!
}

enum ToDosSortOrder {
  deadlineAcs
  deadlineDecs
  dateCompleteAsc
  dateCompleteDesc
  nameAsc
  nameDesc
}

type CategoriesQueries {
  getAll(
    """
    Argument for Get Categories
    """
    like: String

    """
    Argument for Get Categories
    """
    sortOrder: CategoriesSortOrder
  ): [CategoryType]!
  getById(
    """
    Argument for Get Category
    """
    id: Int! = 0
  ): CategoryType!
}

enum CategoriesSortOrder {
  nameAsc
  nameDesc
  dateAsc
  dateDesc
}

type RootMutations {
  toDos: ToDosMutations
  categories: CategoriesMutations
}

type ToDosMutations {
  create(
    """
    argument for Create ToDo
    """
    toDosCreateInputType: ToDosCreateInputType!
  ): ToDoType
  update(
    """
    Argument for Create ToDo
    """
    toDosUpdateInputType: ToDosUpdateInputType!
  ): ToDoType
  remove(
    """
    Argument for Remove ToDo
    """
    id: Int! = 0
  ): ToDoType
}

input ToDosCreateInputType {
  name: String!
  deadline: DateTime
  categoryId: Int
}

input ToDosUpdateInputType {
  id: Int!
  isComplete: Boolean!
  name: String!
  deadline: DateTime
  categoryId: Int
}

type CategoriesMutations {
  create(
    """
    Argument for Create Category
    """
    categoriesCreateInputType: CategoriesCreateInputType!
  ): CategoryType
  update(
    """
    Argument for Update Category
    """
    categoriesUpdateInputType: CategoriesUpdateInputType!
  ): CategoryType
  remove(
    """
    Argument for Remove Category
    """
    id: Int! = 0
  ): CategoryType
}

input CategoriesCreateInputType {
  name: String!
}

input CategoriesUpdateInputType {
  id: Int!
  name: String!
}
`
