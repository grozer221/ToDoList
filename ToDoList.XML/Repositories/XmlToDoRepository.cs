using System.Xml.Linq;
using System.Xml.Serialization;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.XML.Repositories
{
    public class XmlToDoRepository : IToDoRepository
    {
        private const string XmlFileName = "ToDoList.xml";
        private readonly XmlSerializer XmlSerializer;

        public XmlToDoRepository()
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

        public Task<ToDoModel> GetByIdAsync(int id)
        {
            return Task.FromResult(GetById(id));
        }

        public ToDoModel? GetById(int id)
        {
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)XmlSerializer.Deserialize(fs);
                if (data == null || data.ToDos == null)
                    return null;
                return data.ToDos.SingleOrDefault(t => t.Id == id);
            }
        }

        public Task<List<ToDoModel>> GetWithCategoryAsync(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            return Task.FromResult(Get(like, sortOrder, categoryId));
        }

        public List<ToDoModel> Get(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)XmlSerializer.Deserialize(fs);
                if (data == null)
                    data = new DataWrapper();
                if (data.ToDos == null)
                    data.ToDos = new List<ToDoModel>();
                return data.ToDos;
            }
        }

        public Task<ToDoModel> CreateAsync(ToDoModel toDo)
        {
            return Task.FromResult(Create(toDo));
        }

        public ToDoModel Create(ToDoModel toDo)
        {
            DataWrapper? data;
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper?)XmlSerializer.Deserialize(fs);

                if (data == null || data.ToDos == null)
                    toDo.Id = 1;
                else
                    toDo.Id = data.ToDos.Count + 1;

                if (toDo.CategoryId == 0)
                    toDo.CategoryId = null;
                DateTime dateTimeNow = DateTime.Now;
                toDo.CreatedAt = dateTimeNow;
                toDo.UpdatedAt = dateTimeNow;
            }
            using (FileStream fs = new FileStream(XmlFileName, FileMode.Truncate))
            {
                if (data == null)
                    data = new DataWrapper();
                if (data.ToDos == null)
                    data.ToDos = new List<ToDoModel>();
                data.ToDos.Add(toDo);
                XmlSerializer.Serialize(fs, data);
            }
            return toDo;
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
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)XmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(XmlFileName, FileMode.Truncate))
            {
                int toDoIndex = data.ToDos.FindIndex(t => t.Id == toDo.Id);
                data.ToDos[toDoIndex] = toDo;
                XmlSerializer.Serialize(fs, data);
            }
            return toDo;
        }

        public async Task RemoveAsync(int id)
        {
            ToDoModel toDoIsExists = await GetByIdAsync(id);
            if (toDoIsExists == null)
                throw new Exception($"ToDo with id {id} does not exists");

            DataWrapper data;
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                data = (DataWrapper)XmlSerializer.Deserialize(fs);
            }
            using (FileStream fs = new FileStream(XmlFileName, FileMode.Truncate))
            {
                data.ToDos = data.ToDos.Where(t => t.Id != id).ToList();
                XmlSerializer.Serialize(fs, data);
            }
        }
    }
}
