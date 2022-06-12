using System.Xml.Linq;
using System.Xml.Serialization;
using ToDoList.Business.Abstractions;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.XML.Repositories
{
    public class XmlCategoryRepository : ICategoryRepository
    {
        private readonly string xmlFileName;
        private readonly XmlSerializer xmlSerializer;

        public int Take => 3;

        public XmlCategoryRepository(string xmlFileName)
        {
            this.xmlFileName = xmlFileName;
            if (!File.Exists(xmlFileName))
            {
                XDocument document = new XDocument();
                XElement dataWrapper = new XElement(nameof(DataWrapper));
                document.Add(dataWrapper);
                document.Save(xmlFileName);
            }
            xmlSerializer = new XmlSerializer(typeof(DataWrapper));
        }

        public Task<GetEntitiesResponse<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder, int page)
        {
            like ??= string.Empty;
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)xmlSerializer.Deserialize(fs);
                if (data == null)
                    data = new DataWrapper();
                if (data.Categories == null)
                    data.Categories = new List<CategoryModel>();
                var categories = data.Categories.Where(c => c.Name.Contains(like, StringComparison.OrdinalIgnoreCase));
                int total = categories.Count();
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
                int skip = (page - 1) * Take;
                categories.Skip(skip).Take(Take);
                return Task.FromResult(new GetEntitiesResponse<CategoryModel>
                {
                    Entities = categories,
                    Total = total,
                    PageSize = Take,
                });
            }
        }

        public Task<CategoryModel> GetByIdAsync(int id)
        {
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)xmlSerializer.Deserialize(fs);
                if (data == null || data.Categories == null)
                    return null;
                CategoryModel category = data.Categories.SingleOrDefault(t => t.Id == id);
                return Task.FromResult(category);
            }
        }

        public Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            DataWrapper? data;
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper?)xmlSerializer.Deserialize(fs);

                if (data == null || data.Categories == null)
                    category.Id = 1;
                else
                    category.Id = data.Categories.Max(c => c.Id) + 1;

                DateTime dateTimeNow = DateTime.Now;
                category.CreatedAt = dateTimeNow;
                category.UpdatedAt = dateTimeNow;
            }
            using (FileStream fs = new FileStream(xmlFileName, FileMode.Truncate))
            {
                if (data == null)
                    data = new DataWrapper();
                if (data.Categories == null)
                    data.Categories = new List<CategoryModel>();
                data.Categories.Add(category);
                xmlSerializer.Serialize(fs, data);
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
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)xmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(xmlFileName, FileMode.Truncate))
            {
                int categoryIndex = data.Categories.FindIndex(c => c.Id == category.Id);
                data.Categories[categoryIndex] = category;
                xmlSerializer.Serialize(fs, data);
            }
            return category;
        }

        public async Task<CategoryModel> RemoveAsync(int id)
        {
            CategoryModel categoryIsExists = await GetByIdAsync(id);
            if (categoryIsExists == null)
                throw new Exception($"Category with id {id} does not exists");

            DataWrapper data;
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)xmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(xmlFileName, FileMode.Truncate))
            {
                data.Categories = data.Categories.Where(t => t.Id != id).ToList();
                xmlSerializer.Serialize(fs, data);
            }
            return categoryIsExists;
        }
    }
}
