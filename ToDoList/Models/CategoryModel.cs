using System.ComponentModel.DataAnnotations;
using ToDoList.ViewModels.Categories;

namespace ToDoList.Models
{
    public class CategoryModel : BaseModel
    {
        public CategoryModel()
        {
        }

        public CategoryModel(CategoriesCreateViewModel categoriesCreateViewModel)
        {
            Name = categoriesCreateViewModel.Name;
        }

        public CategoryModel(CategoriesEditViewModel categoriesEditViewModel)
        {
            Id = categoriesEditViewModel.Id;
            Name = categoriesEditViewModel.Name;
        }

        public string Name { get; set; }
        public int? UserId { get; set; }
        public UserModel? User { get; set; }
        public List<ToDoModel> ToDos { get; set; } = new List<ToDoModel>();
    }
}
