namespace ToDoList.Domain;

public class ToDoItem
{
    public ToDoItem(Guid id, string name, bool done, DateTime createdAt)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Completed = done;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }
    public string Name { get; }
    public bool Completed { get; }
    public DateTime CreatedAt { get; }

    public static ToDoItem Create(string name) => new(Guid.NewGuid(), name, false, DateTime.Now);

    public ToDoItem Rename(string newName) => new(Id, newName, Completed, CreatedAt);
    public ToDoItem Complete() => new(Id, Name, true, CreatedAt);
    public ToDoItem Incomplete() => new(Id, Name, false, CreatedAt);
}
