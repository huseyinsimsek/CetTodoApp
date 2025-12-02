using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace CetTodoApp
{
    // Veritabanı Tablosu
    [Table("todo_items")]
    public class TodoItem : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }
        public DateTime DueDate { get; set; }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public partial class MainPage : ContentPage
    {
        // Veritabanı Servisimiz
        private readonly TodoService _todoService = new TodoService();

        public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        // Uygulama ekrana geldiğinde çalışır (Verileri yükle)
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadData();
        }

        // Veritabanından verileri çekip ekrana basar
        private async Task LoadData()
        {
            var todoList = await _todoService.GetTodosAsync();

            TodoItems.Clear(); // Ekrandaki listeyi temizle
            foreach (var item in todoList)
            {
                TodoItems.Add(item); // Veritabanından gelenleri ekle
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            // Validasyonlar
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Hata", "Lütfen bir görev yazın.", "Tamam");
                return;
            }

            if (MyDatePicker.Date < DateTime.Now.Date)
            {
                await DisplayAlert("Hata", "Geçmişe görev ekleyemezsiniz!", "Tamam");
                return;
            }

            // Yeni Görev
            var newItem = new TodoItem
            {
                Title = TitleEntry.Text,
                DueDate = MyDatePicker.Date,
                IsCompleted = false
            };

            // 1. Veritabanına Kaydet (Kalıcı olsun)
            await _todoService.SaveTodoAsync(newItem);

            // 2. Listeyi Yenile (Ekrana gelsin)
            await LoadData();

            // Kutuları Temizle
            TitleEntry.Text = string.Empty;
            MyDatePicker.Date = DateTime.Now;
        }
    }
}