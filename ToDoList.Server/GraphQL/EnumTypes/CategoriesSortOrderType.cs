using GraphQL;
using GraphQL.Types;
using ToDoList.Business.Enums;

namespace ToDoList.GraphQL.EnumTypes
{
    public class CategoriesSortOrderType : EnumerationGraphType<CategoriesSortOrder>
    {
        protected override string ChangeEnumCase(string val) => val.ToCamelCase();
    }
}
