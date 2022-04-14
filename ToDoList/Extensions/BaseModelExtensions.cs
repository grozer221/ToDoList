using Pluralize.NET.Core;

namespace ToDoList.Extensions
{
    public static class BaseModelExtensions
    {
        public static string GetTableName(this BaseModel baseModel)
        {
            string modelName = baseModel.GetType().Name.ReplaceInEnd("Model", "");
            return new Pluralizer().Pluralize(modelName); ;
        }
    }
}
