using Habit_Traker.U_riani.Data;
using Habit_Traker.U_riani.Helpers;
using Habit_Traker.U_riani.Model;
using Microsoft.Data.Sqlite;

namespace Habit_Traker.U_riani;
internal class HabitService {
    public static void ViewAll() {
        List<Habit> habits = HabitRepository.GetAll();
        if (habits.Count == 0) {
            Console.WriteLine("No habits found.");
            return;
        }
        Console.WriteLine("List of Habits:\n");
        Console.Write("Id".PadRight(5));
        Console.Write("Name".PadRight(20));
        Console.Write("Quantity".PadRight(20));
        Console.Write("Date\n\n");
        foreach (Habit habit in habits) {
            string quantityWithUnit = $"{habit.Quantity:0.00} {habit.Unit}";
            Console.WriteLine($"{habit.Id,-5}{habit.Name,-20}{quantityWithUnit,-20}{habit.StartDate:dd-MM-yy}");
        }
    }

    public static void AddHabit() {
        string name = ConsoleHelper.GetNonEmptyInput("Enter name of habit: ");
        decimal quantity = ConsoleHelper.GetDecimalInput("Enter Quantity: ");
        string unit = ConsoleHelper.GetNonEmptyInput("Enter unit of habit: ");
        DateTime startDate = ConsoleHelper.GetDateInput("Enter start date (dd-MM-yy): ");

        Habit newHabit = new() {
            Name = name,
            Quantity = quantity,
            Unit = unit,
            StartDate = startDate
        };

        HabitRepository.Add(newHabit);
        Console.WriteLine("Habit added successfully.");
    }

    public static void UpdateHabit() {
        ViewAll();
        int id = ConsoleHelper.GetIntInput("Enter Id of habit to update: ");
        Habit? habit = HabitRepository.GetById(id);

        if (habit == null) {
            Console.WriteLine($"No habit found with ID {id}.");
            return;
        }

        string name = ConsoleHelper.GetNonEmptyInput($"Current Name: {habit.Name}. Enter new name: ");
        decimal quantity = ConsoleHelper.GetDecimalInput($"Current Frequency: {habit.Quantity}. Enter new frequency: ");
        string unit = ConsoleHelper.GetNonEmptyInput($"Current Name: {habit.Unit}. Enter unit name: ");
        DateTime startDate = ConsoleHelper.GetDateInput($"Current Start Date: {habit.StartDate.ToString("dd-MM-yy")}. Enter new start date: ");

        habit.Name = name;
        habit.Quantity = quantity;
        habit.Unit = unit;
        habit.StartDate = startDate;

        if (HabitRepository.Update(habit)) {
            Console.WriteLine("Habit updated successfully.");
        } else {
            Console.WriteLine("Update failed.");
        }
    }

    public static void DeleteHabit() {
        int id = ConsoleHelper.GetIntInput("Enter the ID of the habit to delete: ");
        if (HabitRepository.Delete(id)) {
            Console.WriteLine($"Habit with ID {id} deleted successfully.");
        } else {
            Console.WriteLine($"No habit found with ID {id}.");
        }
    }

    public static void GetReport() {
        int year = ConsoleHelper.GetIntInput("Enter year for report: ");
        while (year.ToString().Length != 4) {
            year = ConsoleHelper.GetIntInput("Invalid year format. Please enter a 4-digit year.");
        }

        List<HabitsReport>? report = HabitRepository.FilterData(year.ToString().Substring(2));

        if (report != null) {
            Console.WriteLine($"Report for 20{year} year");
            for (int i = 0; i < report.Count; i++) {
                HabitsReport habitReport = report[i];
                Console.WriteLine($"{i + 1}. {habitReport.Name} - {habitReport.Total:0.00} {habitReport.Unit}");
            }
        } else {
            Console.WriteLine("No data found for the specified year.");
        }

    }

}

