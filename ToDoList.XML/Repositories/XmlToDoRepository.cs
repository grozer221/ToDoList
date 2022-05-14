using System.Xml.Linq;
using System.Xml.Serialization;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;
using ToDoList.XML.Enums;

namespace ToDoList.XML.Repositories
{
    public class XmlToDoRepository : IToDoRepository
    {
        private readonly string xmlFileName;
        private readonly XmlSerializer xmlSerializer;

        public XmlToDoRepository(string xmlFileName)
        {
            this.xmlFileName = xmlFileName;
            if (!File.Exists(this.xmlFileName))
            {
                XDocument document = new XDocument();
                XElement dataWrapper = new XElement(nameof(XML.DataWrapper));
                document.Add(dataWrapper);
                document.Save(this.xmlFileName);
            }
            xmlSerializer = new XmlSerializer(typeof(DataWrapper));
        }

        public Task<ToDoModel> GetByIdOrDefaultAsync(int id)
        {
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)xmlSerializer.Deserialize(fs);
                if (data == null || data.ToDos == null)
                    return null;
                ToDoModel toDo = data.ToDos.SingleOrDefault(t => t.Id == id);
                return Task.FromResult(toDo);
            }
        }

        public async Task<ToDoModel> GetByIdAsync(int id)
        {
            var toDo = await GetByIdOrDefaultAsync(id);
            if (toDo == null)
                throw new Exception($"ToDo with id {id} not found");
            return toDo;
        }

        public Task<IEnumerable<ToDoModel>> GetWithCategoryOrDefaultAsync(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            like ??= string.Empty;
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)xmlSerializer.Deserialize(fs);
                if (data == null)
                    data = new DataWrapper();
                if (data.ToDos == null)
                    data.ToDos = new List<ToDoModel>();

                var toDos = data.ToDos
                    .Where(t => t.Name.Contains(like, StringComparison.OrdinalIgnoreCase));

                if (categoryId != null && categoryId != 0)
                    toDos = data.ToDos
                        .Where(t => t.CategoryId == categoryId);

                switch (sortOrder)
                {
                    case ToDosSortOrder.DeadlineAcs:
                        toDos = GetOrderedBy(toDos, t => t.Deadline.HasValue, t => t.Deadline, OrderBy.Asc);
                        break;
                    case ToDosSortOrder.DeadlineDecs:
                        toDos = GetOrderedBy(toDos, t => t.Deadline.HasValue, t => t.Deadline, OrderBy.Desc);
                        break;
                    case ToDosSortOrder.NameAsc:
                        toDos = GetOrderedBy(toDos, t => t.Name, t => t.Name, OrderBy.Asc);
                        break;
                    case ToDosSortOrder.NameDesc:
                        toDos = GetOrderedBy(toDos, t => t.Name, t => t.Name, OrderBy.Desc);
                        break;
                    case ToDosSortOrder.DateCompleteAsc:
                        toDos = GetOrderedBy(toDos, t => t.DateComplete.HasValue, t => t.DateComplete, OrderBy.Asc);
                        break;
                    case ToDosSortOrder.DateCompleteDesc:
                        toDos = GetOrderedBy(toDos, t => t.DateComplete.HasValue, t => t.DateComplete, OrderBy.Desc);
                        break;
                }

                foreach (var toDo in toDos)
                {
                    toDo.Category = data.Categories.SingleOrDefault(c => c.Id == toDo.CategoryId);
                }
                return Task.FromResult(toDos);
            }
        }

        public Task<IEnumerable<ToDoModel>> GetWithCategoryAsync(string? like = null, ToDosSortOrder sortOrder = ToDosSortOrder.DeadlineAcs, int? categoryId = null)
        {
            var toDos = GetWithCategoryOrDefaultAsync(like, sortOrder, categoryId);
            if (toDos == null)
                throw new Exception($"ToDos not found");
            return toDos;
        }

        private IEnumerable<ToDoModel> GetOrderedBy(IEnumerable<ToDoModel> toDos, Func<ToDoModel, object> keySelector1, Func<ToDoModel, object> keySelector2, OrderBy orderBy)
        {
            switch (orderBy)
            {
                case OrderBy.Desc:
                    return toDos
                        .OrderBy(t => t.IsComplete)
                        .ThenByDescending(keySelector1)
                        .ThenByDescending(keySelector2);
                case OrderBy.Asc:
                default:
                    return toDos
                        .OrderBy(t => t.IsComplete)
                        .ThenByDescending(keySelector1)
                        .ThenBy(keySelector2);
            }
        }

        public Task<ToDoModel> CreateAsync(ToDoModel toDo)
        {
            DataWrapper? data;
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper?)xmlSerializer.Deserialize(fs);

                if (data == null || data.ToDos == null)
                    toDo.Id = 1;
                else
                    toDo.Id = data.ToDos.Max(t => t.Id) + 1;

                if (toDo.CategoryId == 0)
                    toDo.CategoryId = null;
                DateTime dateTimeNow = DateTime.Now;
                toDo.CreatedAt = dateTimeNow;
                toDo.UpdatedAt = dateTimeNow;
            }
            using (FileStream fs = new FileStream(xmlFileName, FileMode.Truncate))
            {
                if (data == null)
                    data = new DataWrapper();
                if (data.ToDos == null)
                    data.ToDos = new List<ToDoModel>();
                data.ToDos.Add(toDo);
                xmlSerializer.Serialize(fs, data);
            }
            return Task.FromResult(toDo);
        }

        public async Task<ToDoModel> UpdateAsync(ToDoModel toDo)
        {
            ToDoModel toDoIsExists = await GetByIdAsync(toDo.Id);
            if (toDoIsExists == null)
                throw new Exception($"ToDo with id {toDo.Id} does not exists");

            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;

            DateTime dateTimeNow = DateTime.Now;
            if (!toDoIsExists.IsComplete && toDo.IsComplete)
                toDo.DateComplete = dateTimeNow;
            else if (toDoIsExists.IsComplete && !toDo.IsComplete)
                toDo.DateComplete = null;
            else
                toDo.DateComplete = toDoIsExists.DateComplete;

            toDo.CreatedAt = toDoIsExists.CreatedAt;
            toDo.UpdatedAt = dateTimeNow;

            DataWrapper data;
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)xmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(xmlFileName, FileMode.Truncate))
            {
                int toDoIndex = data.ToDos.FindIndex(t => t.Id == toDo.Id);
                data.ToDos[toDoIndex] = toDo;
                xmlSerializer.Serialize(fs, data);
            }
            return toDo;
        }

        public async Task<ToDoModel> RemoveAsync(int id)
        {
            ToDoModel toDoIsExists = await GetByIdAsync(id);
            if (toDoIsExists == null)
                throw new Exception($"ToDo with id {id} does not exists");

            DataWrapper data;
            using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)xmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(xmlFileName, FileMode.Truncate))
            {
                data.ToDos = data.ToDos.Where(t => t.Id != id).ToList();
                xmlSerializer.Serialize(fs, data);
            }
            return toDoIsExists;
        }
    }
}
