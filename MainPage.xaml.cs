using System.Collections.ObjectModel;

namespace CetTodoApp
{
    // Yardımcı sınıf (TodoItem)
    public class TodoItem
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            // Validasyon 1: Boş Başlık Kontrolü
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Hata", "Lütfen yapılacak işi giriniz!", "Tamam");
                return;
            }

            // Validasyon 2: Tarih Kontrolü
            if (MyDatePicker.Date < DateTime.Now.Date)
            {
                await DisplayAlert("Hata", "Geçmişe dönük plan yapamazsınız!", "Tamam");
                return;
            }

            // Listeye Ekleme
            TodoItems.Add(new TodoItem
            {
                Title = TitleEntry.Text,
                DueDate = MyDatePicker.Date,
                IsCompleted = false
            });

            // Temizlik
            TitleEntry.Text = string.Empty;
            MyDatePicker.Date = DateTime.Now;
        }
    }
}