using SQLite;

namespace CetTodoApp
{
    public class TodoService
    {
        // Veritabanı bağlantısı
        private SQLiteAsyncConnection _database;

        // Veritabanını başlatan fonksiyon (Tablo oluşturur)
        async Task Init()
        {
            if (_database is not null)
                return;

            // Veritabanı dosyasını telefonun özel klasöründe oluşturuyoruz
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyTodo.db");

            _database = new SQLiteAsyncConnection(dbPath);
            
            // Tabloyu yaratıyoruz (Eğer yoksa)
            await _database.CreateTableAsync<TodoItem>();
        }

        // Bütün görevleri getir
        public async Task<List<TodoItem>> GetTodosAsync()
        {
            await Init();
            return await _database.Table<TodoItem>().ToListAsync();
        }

        // Yeni görev ekle veya var olanı güncelle
        public async Task<int> SaveTodoAsync(TodoItem item)
        {
            await Init();
            if (item.Id != 0)
                return await _database.UpdateAsync(item); // ID varsa güncelle
            else
                return await _database.InsertAsync(item); // ID yoksa (0 ise) yeni ekle
        }

        // Görev sil
        public async Task<int> DeleteTodoAsync(TodoItem item)
        {
            await Init();
            return await _database.DeleteAsync(item);
        }
    }
}