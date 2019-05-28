import _ from "lodash";

export default class TodoList {  

  load() {    
    return fetch("api/todoitem").then(res => res.json());
  }

  add(name) {    
    return fetch("api/todoitem", { method: "POST", body: JSON.stringify(name), headers: {'Content-Type': 'application/json'}});
  }

  delete(todo) {
    return fetch("api/todoitem/" + todo.id, { method: "DELETE"});
  }

  toggle(todo) {
    let item = _.find(this.items, it => it.id === todo.id);
    if (item) {
      item.completed = !item.completed;
      if (item.completed) {
        item.completedAt = Date.now();
      }
    }
  }

  rename(id, newName) {
    let item = _.find(this.items, it => it.id === id);
    if (item) {
      item.name = newName;      
    }
  }

  filter(status, items) {
    switch (status) {
      case "active":
        return items.filter(item => item.completed === false);
      case "completed":
        return items.filter(item => item.completed === true);
      case "all":
      default:
        return items;
    }
  }
}
