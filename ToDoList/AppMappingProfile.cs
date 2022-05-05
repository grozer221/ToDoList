using AutoMapper;
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
            CreateMap<ToDoModel, ToDosCreateViewModel>().ReverseMap();
            CreateMap<ToDoModel, ToDoEditViewModel>().ReverseMap();
            CreateMap<ToDoModel, ToDosDeleteViewModel>().ReverseMap();

            CreateMap<CategoryModel, CategoriesIndexViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesDetailsViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesCreateViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesEditViewModel>().ReverseMap();
            CreateMap<CategoryModel, CategoriesDeleteViewModel>().ReverseMap();
        }
    }
}