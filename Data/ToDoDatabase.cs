using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SQLite;

namespace CetTodoApp.Data;

public class ToDoDatabase
{
    private SQLiteAsyncConnection? _database;

    private async Task InitAsync()
    {
        if (_database is not null)
        {
            return;
        }

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "todos.db3");
        _database = new SQLiteAsyncConnection(databasePath);
        await _database.CreateTableAsync<TodoItem>();
    }

    public async Task<List<TodoItem>> GetRecentItemsAsync()
    {
        await InitAsync();

        var cutoff = DateTime.Now.AddDays(-1);
        return await _database!
            .Table<TodoItem>()
            .Where(x => !x.IsComplete || (x.IsComplete && x.DueDate > cutoff))
            .OrderBy(x => x.DueDate)
            .ToListAsync();
    }

    public async Task<List<TodoItem>> GetAllAsync()
    {
        await InitAsync();
        return await _database!.Table<TodoItem>().OrderBy(x => x.DueDate).ToListAsync();
    }

    public async Task<int> SaveAsync(TodoItem item)
    {
        await InitAsync();
        if (item.Id != 0)
        {
            return await _database!.UpdateAsync(item);
        }

        return await _database!.InsertAsync(item);
    }

    public async Task<int> ToggleCompletionAsync(TodoItem item)
    {
        await InitAsync();
        item.IsComplete = !item.IsComplete;
        return await _database!.UpdateAsync(item);
    }
}
