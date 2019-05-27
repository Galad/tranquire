using System;
using System.Collections.Generic;

namespace ToDoList.Domain
{
    public class InMemoryToDoItemRepository : IToDoItemRepository
    {
        private readonly Dictionary<Guid, ToDoItem> _items = new Dictionary<Guid, ToDoItem>();

        public void Add(ToDoItem toDoItem)
        {
            _items.Add(toDoItem.Id, toDoItem);
        }

        public void Delete(Guid id)
        {
            _items.Remove(id);
        }

        public ToDoItem Get(Guid id)
        {
            return _items[id];
        }

        public IEnumerable<ToDoItem> Get()
        {
            return _items.Values;
        }

        public void Update(ToDoItem toDoItem)
        {
            _items[toDoItem.Id] = toDoItem;
        }
    }
}
