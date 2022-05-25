using GraphQL;
using GraphQL.Types;
using ToDoList.Business.Enums;

namespace ToDoList.GraphQL.EnumTypes
{
    public class ToDosSortOrderType : EnumerationGraphType<ToDosSortOrder>
    {
        protected override string ChangeEnumCase(string val) => val.ToCamelCase();
    }
}
