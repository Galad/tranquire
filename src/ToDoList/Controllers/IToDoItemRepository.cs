using System;
using System.Collections.Generic;

namespace ToDoList.Domain
{
    public interface IToDoItemRepository
    {
        ToDoItem Get(Guid id);
        IEnumerable<ToDoItem> Get();
        void Update(ToDoItem toDoItem);
        void Add(ToDoItem toDoItem);
        void Delete(Guid id);
    }
}
