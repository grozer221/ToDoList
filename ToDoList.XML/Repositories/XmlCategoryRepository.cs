using System.Xml.Linq;
using System.Xml.Serialization;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.XML.Repositories
{
    public class XmlCategoryRepository : ICategoryRepository
    {
        private const string XmlFileName = "ToDoList.xml";
        private readonly XmlSerializer XmlSerializer;

        public XmlCategoryRepository()
        {
            if (!File.Exists(XmlFileName))
            {
                XDocument document = new XDocument();
                XElement dataWrapper = new XElement(nameof(DataWrapper));
                document.Add(dataWrapper);
                document.Save(XmlFileName);
            }
            XmlSerializer = new XmlSerializer(typeof(DataWrapper));
        }

        public Task<List<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder)
        {
            like ??= string.Empty;
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)XmlSerializer.Deserialize(fs);
                if (data == null)
                    data = new DataWrapper();
                if (data.Categories == null)
                    data.Categories = new List<CategoryModel>();
                var categories = data.Categories.Where(c => c.Name.Contains(like, StringComparison.OrdinalIgnoreCase));
                switch (sortOrder)
                {
                    case CategoriesSortOrder.NameAsc:
                        categories = categories.OrderBy(c => c.Name);
                        break;
                    case CategoriesSortOrder.NameDesc:
                        categories = categories.OrderByDescending(c => c.Name);
                        break;
                    case CategoriesSortOrder.DateAsc:
                        categories = categories.OrderBy(c => c.CreatedAt);
                        break;
                    case CategoriesSortOrder.DateDesc:
                        categories = categories.OrderByDescending(c => c.CreatedAt);
                        break;
                }
                return Task.FromResult(categories.ToList());
            }
        }

        public Task<CategoryModel> GetByIdAsync(int id)
        {
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)XmlSerializer.Deserialize(fs);
                if (data == null || data.Categories == null)
                    return null;
                CategoryModel category = data.Categories.SingleOrDefault(t => t.Id == id);
                return Task.FromResult(category);
            }
        }

        public async Task<CategoryModel> GetByIdWithTodosAsync(int id)
        {
            CategoryModel category = await GetByIdAsync(id);
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)XmlSerializer.Deserialize(fs);
                if (data == null || data.ToDos == null)
                    return null;
                category.ToDos = data.ToDos.Where(t => t.Id == id).ToList();
                return category;
            }
        }

        public Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            DataWrapper? data;
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper?)XmlSerializer.Deserialize(fs);

                if (data == null || data.Categories == null)
                    category.Id = 1;
                else
                    category.Id = data.ToDos.Count + 1;

                DateTime dateTimeNow = DateTime.Now;
                category.CreatedAt = dateTimeNow;
                category.UpdatedAt = dateTimeNow;
            }
            using (FileStream fs = new FileStream(XmlFileName, FileMode.Truncate))
            {
                if (data == null)
                    data = new DataWrapper();
                if (data.Categories == null)
                    data.Categories = new List<CategoryModel>();
                data.Categories.Add(category);
                XmlSerializer.Serialize(fs, data);
            }
            return Task.FromResult(category);
        }

        public async Task<CategoryModel> UpdateAsync(CategoryModel category)
        {
            CategoryModel categoryIsExists = await GetByIdAsync(category.Id);
            if (categoryIsExists == null)
                throw new Exception($"Category with id {category.Id} does not exists");

            DateTime dateTimeNow = DateTime.Now;
            category.CreatedAt = categoryIsExists.CreatedAt;
            category.UpdatedAt = dateTimeNow;

            DataWrapper data;
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)XmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(XmlFileName, FileMode.Truncate))
            {
                int categoryIndex = data.Categories.FindIndex(c => c.Id == category.Id);
                data.Categories[categoryIndex] = category;
                XmlSerializer.Serialize(fs, data);
            }
            return category;
        }

        public async Task RemoveAsync(int id)
        {
            CategoryModel categoryIsExists = await GetByIdAsync(id);
            if (categoryIsExists == null)
                throw new Exception($"Category with id {id} does not exists");

            DataWrapper data;
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)XmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(XmlFileName, FileMode.Truncate))
            {
                data.Categories = data.Categories.Where(t => t.Id != id).ToList();
                XmlSerializer.Serialize(fs, data);
            }
        }
    }
}
