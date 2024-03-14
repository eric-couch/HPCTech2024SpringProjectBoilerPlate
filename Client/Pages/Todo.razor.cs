using HPCTech2024SpringProjectBoilerPlate.Shared;

namespace HPCTech2024SpringProjectBoilerPlate.Client.Pages;

public partial class Todo
{
    private List<TodoItem> todoItems = new List<TodoItem>();
    private string? newTodo;

    private void AddTodo()
    {
        if (!string.IsNullOrWhiteSpace(newTodo))
        {
            todoItems.Add(new TodoItem { Title = newTodo });
            newTodo = string.Empty;
        }
    }
}
