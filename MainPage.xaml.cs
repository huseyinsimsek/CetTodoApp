using CetTodoApp.Data;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
   

    public MainPage()
    {
        InitializeComponent();
        FakeDb.AddToDo("Test1" ,DateTime.Now.AddDays(-1));
        FakeDb.AddToDo("Test2" ,DateTime.Now.AddDays(1));
        FakeDb.AddToDo("Test3" ,DateTime.Now);
        RefreshListView();
        ;


    }


    private void AddButton_OnClicked(object? sender, EventArgs e)
    {
        // --- VALIDASYON MANTIĞI BAŞLANGIÇ ---
        bool isValid = true;
        
        // Hata etiketlerini görünmez yap (Yeni deneme için)
        TitleErrorLabel.IsVisible = false;
        DueDateErrorLabel.IsVisible = false;

        // 1. Title Validasyonu (Boş Kontrolü)
        if (string.IsNullOrWhiteSpace(Title.Text))
        {
            TitleErrorLabel.IsVisible = true;
            isValid = false;
        }

        // 2. Due Date Validasyonu (Geçmiş Tarih Kontrolü)
        if (DueDate.Date < DateTime.Now.Date)
        {
            DueDateErrorLabel.IsVisible = true;
            isValid = false;
        }

        // Eğer herhangi bir doğrulama başarısızsa (isValid == false), metottan çık (return).
        if (!isValid)
        {
            return; 
        }
        // --- VALIDASYON MANTIĞI BİTİŞ ---

        // Validasyon başarılıysa, orijinal kaydetme kodu çalışır:
        FakeDb.AddToDo(Title.Text, DueDate.Date);
        Title.Text = string.Empty;
        DueDate.Date = DateTime.Now;
        RefreshListView();
    }

    private void RefreshListView()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = FakeDb.Data.Where(x => !x.IsComplete ||
                                                           (x.IsComplete && x.DueDate > DateTime.Now.AddDays(-1)))
            .ToList();
    }

    private void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        var item = e.SelectedItem as TodoItem;
       FakeDb.ChageCompletionStatus(item);
       RefreshListView();
       
    }
}