using CetTodoApp.Data;

using System.Threading.Tasks;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
    private readonly ToDoDatabase _database;

    public MainPage(ToDoDatabase database)
    {
        InitializeComponent();
        _database = database;

        InitializeDataAsync();
    }

    private async void InitializeDataAsync()
    {
        var items = await _database.GetAllAsync();

        if (items.Count == 0)
        {
            await _database.SaveAsync(new TodoItem { Title = "Test1", DueDate = DateTime.Now.AddDays(-1) });
            await _database.SaveAsync(new TodoItem { Title = "Test2", DueDate = DateTime.Now.AddDays(1) });
            await _database.SaveAsync(new TodoItem { Title = "Test3", DueDate = DateTime.Now });
        }

        await RefreshListViewAsync();
    }

    private async void AddButton_OnClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Title.Text))
        {
            return;
        }

        await _database.SaveAsync(new TodoItem { Title = Title.Text, DueDate = DueDate.Date });
        Title.Text = string.Empty;
        DueDate.Date = DateTime.Now;
        await RefreshListViewAsync();
    }

    private async Task RefreshListViewAsync()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = await _database.GetRecentItemsAsync();
    }

    private async void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is not TodoItem item)
        {
            return;
        }

        await _database.ToggleCompletionAsync(item);
        TasksListView.SelectedItem = null;
        await RefreshListViewAsync();
    }
}
