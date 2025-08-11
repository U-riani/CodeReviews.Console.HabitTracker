using Habit_Traker.U_riani.Helpers;
using Habit_Traker.U_riani.Model;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Xml.Linq;
using static Habit_Traker.U_riani.Helpers.ConsoleHelper;

namespace Habit_Traker.U_riani.Data {
    public class HabitRepository {
        private const string connectionString = "Data Source=habits.db";

        public static void InitializeDb() {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {
                    tableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Quantity REAL NOT NULL,
                        Unit TEXT,
                        StartDate TEXT NOT NULL
                    )";

                    tableCmd.ExecuteNonQuery();
                }

            }

            InsertInitialData();
        }

        private static void InsertInitialData() {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {
                    tableCmd.CommandText = @"SELECT COUNT(Id) AS ROWS 
                                            FROM Habits";
                    //tableCmd.ExecuteNonQuery();

                    int rows = Convert.ToInt32(tableCmd.ExecuteScalar());
                    Console.WriteLine(rows);

                    if (rows < 1) {
                        for (int i = 0; i < 100; i++) {
                            HabitClass newHabit = ConsoleHelper.GenerateRandomRecords();
                            using (SqliteCommand tableCmdToInsertInitialData = connection.CreateCommand()) {
                                tableCmdToInsertInitialData.CommandText =
                                    @"INSERT INTO Habits (Name, Quantity, Unit, StartDate)
                                    values(@name, @quantity, @unit, @startDate)";
                                tableCmdToInsertInitialData.Parameters.AddWithValue("@name", newHabit.Name);
                                tableCmdToInsertInitialData.Parameters.AddWithValue("@quantity", newHabit.Quantity);
                                tableCmdToInsertInitialData.Parameters.AddWithValue("@unit", newHabit.Unit);
                                tableCmdToInsertInitialData.Parameters.AddWithValue("@startDate", newHabit.Date.ToString("dd-MM-yy"));
                                tableCmdToInsertInitialData.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        public static void Add(Habit habit) {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {
                    tableCmd.CommandText = @"INSERT INTO Habits (Name, Quantity, Unit, StartDate) 
                                            values (@name, @quantity, @unit, @startDate)";
                    tableCmd.Parameters.AddWithValue("@name", habit.Name);
                    tableCmd.Parameters.AddWithValue("@quantity", habit.Quantity);
                    tableCmd.Parameters.AddWithValue("@unit", habit.Unit);
                    tableCmd.Parameters.AddWithValue("@startDate", habit.StartDate.ToString("dd-MM-yy"));
                    tableCmd.ExecuteNonQuery();


                }
            }
        }

        public static List<Habit> GetAll() {
            List<Habit> habits = new List<Habit>();

            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {
                    tableCmd.CommandText = "SELECT * FROM Habits";

                    using (SqliteDataReader reader = tableCmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Habit habitRow = new() {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Quantity = reader.GetDecimal(2),
                                Unit = reader.GetString(3),
                                StartDate = DateTime.ParseExact(reader.GetString(4), "dd-MM-yy", CultureInfo.InvariantCulture)
                            };
                            habits.Add(habitRow);
                        }

                    }
                }

            }
            return habits;
        }


        public static bool Update(Habit habit) {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {
                    tableCmd.CommandText = @"UPDATE Habits 
                                            SET Name = @name, Quantity = @quantity, Unit = @unit, StartDate = @startDate 
                                            WHERE Id = @id";
                    tableCmd.Parameters.AddWithValue("@id", habit.Id);
                    tableCmd.Parameters.AddWithValue("@name", habit.Name);
                    tableCmd.Parameters.AddWithValue("@quantity", habit.Quantity);
                    tableCmd.Parameters.AddWithValue("@unit", habit.Unit);
                    tableCmd.Parameters.AddWithValue("@startDate", habit.StartDate.ToString("dd-MM-yy"));

                    return tableCmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool Delete(int id) {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {
                    tableCmd.CommandText = "DELETE FROM Habits WHERE Id = @id";
                    tableCmd.Parameters.AddWithValue("@id", id);

                    return tableCmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static Habit? GetById(int id) {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {
                    tableCmd.CommandText = "SELECT * FROM Habits WHERE Id = @id";
                    tableCmd.Parameters.AddWithValue("@id", id);

                    using (SqliteDataReader reader = tableCmd.ExecuteReader()) {
                        if (reader.Read()) {
                            return new Habit {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Quantity = reader.GetInt32(2),
                                Unit = reader.GetString(3),
                                StartDate = DateTime.ParseExact(reader.GetString(4), "dd-MM-yy", CultureInfo.InvariantCulture)
                            };
                        }
                    }
                }
            }
            return null; // Return null if no habit found with the given id
        }

        public static List<HabitsReport>? FilterData(string selectedYear) {
            List<HabitsReport> habitsReport = new List<HabitsReport>();

            using (SqliteConnection connection = new SqliteConnection(connectionString)) {
                connection.Open();

                using (SqliteCommand tableCmd = connection.CreateCommand()) {

                    tableCmd.CommandText = @"
                        SELECT Name, SUM(Quantity) AS Total, Unit
                        FROM Habits
                        WHERE substr(StartDate, - 2) = @year
                        GROUP BY Name, Unit
                        HAVING Total > 0
                        ";

                    tableCmd.Parameters.AddWithValue("@year", selectedYear);

                    using (SqliteDataReader reader = tableCmd.ExecuteReader()) {
                        while (reader.Read()) {
                            HabitsReport reportRow = new() {
                                Name = reader.GetString(0),
                                Total = reader.GetDecimal(1),
                                Unit = reader.GetString(2)
                            };

                            habitsReport.Add(reportRow);
                        }
                    }

                }
            }
            return habitsReport.Count > 0 ? habitsReport : null;
        }
    }
}

