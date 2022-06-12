using GraphQL.Types;
using ToDoList.Business.Abstractions;

namespace ToDoList.Server.GraphQL.Types
{
    public class GetEntitiesResponseType<TGraphType, TReturnType> : ObjectGraphType<GetEntitiesResponse<TReturnType>>
        where TGraphType : ObjectGraphType<TReturnType> 
        where TReturnType : BaseModel
    {
        public GetEntitiesResponseType()
        {
            Name = $"Get{typeof(TReturnType).Name.Replace("Model", "")}ResponseType";

            Field<NonNullGraphType<ListGraphType<TGraphType>>, IEnumerable<TReturnType>>()
                .Name("Entities")
                .Resolve(context => context.Source.Entities);

            Field<NonNullGraphType<IntGraphType>, int>()
                .Name("Total")
                .Resolve(context => context.Source.Total);

            Field<NonNullGraphType<IntGraphType>, int>()
                .Name("PageSize")
                .Resolve(context => context.Source.PageSize);
        }
    }
}
