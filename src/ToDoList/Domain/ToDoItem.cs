using System;

namespace ToDoList.Domain
{
    public class ToDoItem
    {
        public ToDoItem(Guid id, string name, bool done)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Done = done;
        }

        public Guid Id { get; }
        public string Name { get; }
        public bool Done { get; }

        public ToDoItem Rename(string newName) => new ToDoItem(Id, newName, Done);
        public ToDoItem Complete() => new ToDoItem(Id, Name, true);
        public ToDoItem Incomplete() => new ToDoItem(Id, Name, false);
    }
}
