using AutoMapper;
using ToDoList.ViewModels.Categories;
using ToDoList.ViewModels.ToDos;
using static ToDoList.ViewModels.ToDos.ToDosCreateViewModel;
using static ToDoList.ViewModels.ToDos.ToDosIndexViewModel;

namespace ToDoList
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ToDoModel, ToDoIndexViewModel>();
            CreateMap<ToDoIndexViewModel, ToDoModel>();
            
            CreateMap<ToDoModel, ToDoCreateViewModel>();
            CreateMap<ToDoCreateViewModel, ToDoModel>();
            
            CreateMap<ToDoModel, ToDoEditViewModel>();
            CreateMap<ToDoEditViewModel, ToDoModel>();
            
            CreateMap<ToDoModel, ToDosDeleteViewModel>();
            CreateMap<ToDosDeleteViewModel, ToDoModel>();


            CreateMap<CategoryModel, CategoriesIndexViewModel>();
            CreateMap<CategoriesIndexViewModel, CategoryModel>();

            CreateMap<CategoryModel, CategoriesDetailsViewModel>();
            CreateMap<CategoriesDetailsViewModel, CategoryModel>();

            CreateMap<CategoryModel, CategoriesCreateViewModel>();
            CreateMap<CategoriesCreateViewModel, CategoryModel>();

            CreateMap<CategoryModel, CategoriesEditViewModel>();
            CreateMap<CategoriesEditViewModel, CategoryModel>();
            
            CreateMap<CategoryModel, CategoriesDeleteViewModel>();
            CreateMap<CategoriesDeleteViewModel, CategoryModel>();
        }
    }
}
