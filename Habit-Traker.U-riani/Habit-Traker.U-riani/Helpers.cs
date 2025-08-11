
namespace Habit_Traker.U_riani.Helpers;
public static class ConsoleHelper {
    static Random random = new Random();
    public static void ShowMenu() {
        Console.WriteLine("\n==== Habit Tracker ====");
        Console.WriteLine("1. View all habits");
        Console.WriteLine("2. Add new habit");
        Console.WriteLine("3. Update existing habit");
        Console.WriteLine("4. Delete habit");
        Console.WriteLine("5. View reports by year");
        Console.WriteLine("6. Exit");
        Console.WriteLine("========================");
    }

    public static decimal GetDecimalInput(string prompt) {
        Console.Write(prompt);
        decimal result;
        while (!decimal.TryParse(Console.ReadLine(), out result)) {
            Console.Write("Invalid input. Try again: ");
        }
        return result;
    }

    public static int GetIntInput(string prompt) {
        Console.Write(prompt);
        int result;
        while (!int.TryParse(Console.ReadLine(), out result)) {
            Console.Write("Invalid input. Try again: ");
        }
        return result;
    }

    public static DateTime GetDateInput(string prompt) {
        Console.Write(prompt);
        DateTime date;
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yy", null,
                   System.Globalization.DateTimeStyles.None, out date)) {
            Console.Write("Invalid date. Format: dd-MM-yy. Try again: ");
        }
        return date;
    }

    public static string GetNonEmptyInput(string prompt) {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input)) {
            Console.Write("Input cannot be empty. Try again: ");
            input = Console.ReadLine();
        }
        return input;
    }

    public static void PrintSeparator() {
        Console.WriteLine(new string('-', 30));
    }

    private static readonly Dictionary<string, string> habitUnits = new Dictionary<string, string>() {
        { "Drinking water", "Liters" },
        { "Walking", "Kilometers" },
        { "Eating junk food", "Times" },
        { "Playing games", "Hours" },
        { "Reading", "Pages" }
    };

    public static HabitClass GenerateRandomRecords() {
        // Pick a random habit name
        var habitNames = habitUnits.Keys.ToList();
        string name = habitNames[random.Next(habitNames.Count)];

        // Get the unit associated with the chosen habit name
        string unit = habitUnits[name];

        decimal quantity;
        string[] wholeUnits = { "Pages", "Pushups", "Steps", "Times" };


        if (wholeUnits.Contains(unit)) {
            quantity = Random.Shared.Next(1, 51); // Whole number only
        } else {
            quantity = Math.Round((decimal)(Random.Shared.NextDouble() * 10 + 1), 2); // Decimal number
        }
        DateTime date = GenerateRandomDate();

        return new HabitClass(name, quantity, unit, date);
    }

    public class HabitClass {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime Date { get; set; }

        public HabitClass(string name, decimal quantity, string unit, DateTime date) {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Date = date;
        }

    }
    public static DateTime GenerateRandomDate() {
        Random random = new Random();
        DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
        DateTime end = DateTime.Today;

        int range = (end - start).Days;

        int randomDays = random.Next(0, range + 1);

        DateTime randomDate = start.AddDays(randomDays);

        return randomDate;
    }

}


