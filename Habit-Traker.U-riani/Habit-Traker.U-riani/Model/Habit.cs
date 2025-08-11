
namespace Habit_Traker.U_riani.Model;
public class Habit {
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public DateTime StartDate { get; set; }
}

public class HabitsReport {
    public string? Name { get; set; }
    public decimal Total { get; set; }
    public string? Unit { get; set; }

}

