export type GetEntitiesResponse<T> = {
    entities: T[],
    pageSize: number,
    total: number,
}