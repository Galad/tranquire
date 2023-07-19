using System;
using System.Collections.Generic;

namespace ToDoList.Automation.Model;

/// <summary>
/// Represents a to do item
/// </summary>
public sealed class ToDoItem : IEquatable<ToDoItem>
{
    public ToDoItem(Guid id, string name, bool completed)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Completed = completed;
        Id = id;
    }

    public string Name { get; }

    public bool Completed { get; }

    public Guid Id { get; }

    public override bool Equals(object obj)
    {
        return Equals(obj as ToDoItem);
    }

    public bool Equals(ToDoItem other)
    {
        return other != null &&
               Name == other.Name &&
               Completed == other.Completed;
    }

    public override int GetHashCode()
    {
        var hashCode = -1092317192;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + Completed.GetHashCode();
        return hashCode;
    }

    public static bool operator ==(ToDoItem left, ToDoItem right)
    {
        return EqualityComparer<ToDoItem>.Default.Equals(left, right);
    }

    public static bool operator !=(ToDoItem left, ToDoItem right)
    {
        return !(left == right);
    }
}
