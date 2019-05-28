using System;

namespace ToDoList.Domain
{
    public class ToDoItem
    {
        public ToDoItem(Guid id, string name, bool done, DateTime createdAt)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Completed = done;
            this.CreatedAt = createdAt;
        }

        public Guid Id { get; }
        public string Name { get; }
        public bool Completed { get; }
        public DateTime CreatedAt { get; }

        public static ToDoItem Create(string name) => new ToDoItem(Guid.NewGuid(), name, false, DateTime.Now);

        public ToDoItem Rename(string newName) => new ToDoItem(Id, newName, Completed, CreatedAt);
        public ToDoItem Complete() => new ToDoItem(Id, Name, true, CreatedAt);
        public ToDoItem Incomplete() => new ToDoItem(Id, Name, false, CreatedAt);
    }
}
