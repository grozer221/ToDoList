using System.Xml.Linq;
using System.Xml.Serialization;
using ToDoList.Business.Abstractions;
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

        public int Take => 3;

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

        public Task<ToDoModel> GetByIdAsync(int id)
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

        public Task<GetEntitiesResponse<ToDoModel>> GetAsync(int page = 1, string? like = null, ToDosSortOrder sortOrder = ToDosSortOrder.DeadlineAcs, int? categoryId = null)
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

                int total = toDos.Count();
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
                int skip = (page - 1) * Take;
                toDos.Skip(skip).Take(Take);
                return Task.FromResult(new GetEntitiesResponse<ToDoModel>
                {
                    Entities = toDos,
                    PageSize = Take,
                    Total = total,
                });
            }
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
