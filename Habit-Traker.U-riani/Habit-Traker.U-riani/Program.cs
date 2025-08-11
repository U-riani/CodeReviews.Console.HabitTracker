
using Habit_Traker.U_riani;
using Habit_Traker.U_riani.Data;
using Habit_Traker.U_riani.Helpers;

HabitRepository.InitializeDb();

bool exit = false;

while (!exit) {
    ConsoleHelper.ShowMenu();
    string choice = ConsoleHelper.GetNonEmptyInput("Choose an option: ");

    switch (choice) {
        case "1":
            HabitService.ViewAll();
            break;

        case "2":
            HabitService.AddHabit();
            break;
        case "3":
            HabitService.UpdateHabit();
            break;
        case "4":
            HabitService.DeleteHabit();
            break;
        case "5":
            HabitService.GetReport();
            break;
        case "6":
            exit = true;
            Console.WriteLine("Exiting the application. Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }


}

