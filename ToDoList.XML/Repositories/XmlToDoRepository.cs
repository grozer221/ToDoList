﻿using System.Xml.Linq;
using System.Xml.Serialization;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;
using ToDoList.XML.Enums;

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
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)XmlSerializer.Deserialize(fs);
                if (data == null || data.ToDos == null)
                    return null;
                ToDoModel toDo = data.ToDos.SingleOrDefault(t => t.Id == id);
                return Task.FromResult(toDo);
            }
        }

        public Task<List<ToDoModel>> GetWithCategoryAsync(string? like = null, ToDosSortOrder sortOrder = ToDosSortOrder.DeadlineAcs, int? categoryId = null)
        {
            like ??= string.Empty;
            using (FileStream fs = new FileStream(XmlFileName, FileMode.OpenOrCreate))
            {
                DataWrapper? data = (DataWrapper?)XmlSerializer.Deserialize(fs);
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
                return Task.FromResult(toDos.ToList());
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
