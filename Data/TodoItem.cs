namespace CetTodoApp.Data;

public class TodoItem
{
    public int Id {get; set;}
    public string? Title {get; set;}
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate {get; set;}
    public bool IsComplete { get; set; }

    // YENİ EKLENEN KOD: Süresi geçmiş ve tamamlanmamış öğeleri tespit etmek için
    public bool IsOverdue 
    {
        get
        {
            // Görev tamamlanmadıysa (IsComplete == false) VE Son Tarih bugünden öncesiyse
            return !IsComplete && DueDate.Date < DateTime.Now.Date;
        }
    }

    public TodoItem()
    {
        CreatedDate = DateTime.Now;
        IsComplete = false;
        Title = "";
    }
    
}