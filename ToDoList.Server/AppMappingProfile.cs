using AutoMapper;
using ToDoList.GraphQL.Modules.Categories.DTO;
using ToDoList.GraphQL.Modules.ToDos.DTO;

namespace ToDoList
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ToDoModel, ToDosCreateInput>().ReverseMap();
            CreateMap<ToDoModel, ToDosUpdateInput>().ReverseMap();

            CreateMap<CategoryModel, CategoriesCreateInput>().ReverseMap();
            CreateMap<CategoryModel, CategoriesUpdateInput>().ReverseMap();
        }
    }
}