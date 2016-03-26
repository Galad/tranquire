using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Automation.Model
{
    public sealed class ToDoItem
    {
        public string Name { get; }
        public bool IsCompleted { get; }

        /// <summary>Record Constructor</summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="isCompleted"><see cref="IsCompleted"/></param>
        public ToDoItem(string name, bool isCompleted)
        {            
            Name = name;
            IsCompleted = isCompleted;
        }
    }
}
