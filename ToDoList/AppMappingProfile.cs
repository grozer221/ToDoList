using AutoMapper;
using ToDoList.GraphQL.Modules.Categories.DTO;
using ToDoList.GraphQL.Modules.ToDos.DTO;
using ToDoList.ViewModels.Categories;
using ToDoList.ViewModels.ToDos;
using static ToDoList.ViewModels.ToDos.ToDosIndexViewModel;

namespace ToDoList
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ToDoModel, ToDoIndexViewModel>().ReverseMap();
            CreateMap<ToDoModel, ToDosDetailsViewModel>().ReverseMap();
            CreateMap<ToDoModel, ToDosCreateViewModel>().ReverseMap();
            CreateMap<ToDoModel, ToDoEditViewModel>().ReverseMap();
            CreateMap<ToDoModel, ToDosDeleteViewModel>().ReverseMap();

            CreateMap<CategoryModel, CategoriesIndexViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesDetailsViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesCreateViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesEditViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesDeleteViewModel>().ReverseMap();

            CreateMap<ToDoModel, ToDosCreateInput>().ReverseMap();
            CreateMap<ToDoModel, ToDosUpdateInput>().ReverseMap();

            CreateMap<CategoryModel, CategoriesCreateInput>().ReverseMap();
            CreateMap<CategoryModel, CategoriesUpdateInput>().ReverseMap();
        }
    }
}