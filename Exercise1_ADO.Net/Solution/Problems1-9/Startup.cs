using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Problem1
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string databaseName = "MinionsDB";

                bool databaseExists = CheckDatabaseExists(connection, databaseName);

                if (databaseExists == false)
                {   //1. Initial Setup 
                    string commandText = $"CREATE DATABASE {databaseName}";

                    ExecuteNonQuery(connection, commandText);

                    UseDatabase(connection, databaseName);

                    CreateTables(connection);

                    InsertInitialValuesIntoTables(connection);
                }

                UseDatabase(connection, databaseName);

                // 2. Villain Names
                // Write a program that prints on the console all villains’ names and their
                // number of minions of those who have more than 3 minions ordered descending 
                // by number of minions.

                 GetVillainMinionsCount(connection);


                //3. Minion Names
                // Write a program that prints on the console all minion names and age for a given
                //    villain id, ordered by name alphabetically.
                // If there is no villain with the given ID, print
                // "No villain with ID <VillainId> exists in the database.".
                // If the selected villain has no minions, print "(no minions)" on the second row.

                Task3(connection);


                // 4. Add Minion
                // input fromat: Minion: <Name> <Age> <TownName>

                   Task4(connection);

                // 5. Change Town Names  Casing

                Task5(connection);

                //6. Remove Villain

                Task6(connection);

                //7. Print All Minion Names

                Task7(connection);
                
            }
        }

        private static void Task7(SqlConnection connection)
        {
            List<string> minions = GetAllMinions(connection);

            while (minions.Count > 0)
            {
                Console.WriteLine(minions[0]);

                minions.RemoveAt(0);

                if (minions.Count > 0)
                {
                    Console.WriteLine(minions[minions.Count - 1]);

                    minions.RemoveAt(minions.Count - 1);
                }
            }
        }

        private static List<string> GetAllMinions(SqlConnection connection)
        {
            List<string> minions = new List<string>();

            string getMinionsSql = "SELECT [Name] FROM Minions";

            using (SqlCommand cmd = new SqlCommand(getMinionsSql, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minions.Add((string)reader[0]);
                    }
                }
            }


            return minions;
        }

        private static void Task6(SqlConnection connection)
        {
            int villainId = int.Parse(Console.ReadLine());

            string villainName = GetVillainNameById(connection, villainId);

            if (villainName == null)
            {
                Console.WriteLine("No such villain was found.");
            }
            else
            {
                int releasedMinionsCount = DeleteMinionsVillains(connection, villainId);

                DeleteVillain(connection, villainId);
                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{releasedMinionsCount} minions were released.");
            }

                        ;
        }

        private static void DeleteVillain(SqlConnection connection, int villainId)
        {
            string deleteVillainSql = "DELETE FROM Villains WHERE Id = @villainId";

            using (SqlCommand cmd = new SqlCommand(deleteVillainSql, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);

                cmd.ExecuteNonQuery();
            }
        }

        private static int DeleteMinionsVillains(SqlConnection connection, int villainId)
        {
            string deleteMinionsVillainsSql = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";

            using (SqlCommand cmd = new SqlCommand(deleteMinionsVillainsSql, connection))
            {
                cmd.Parameters.AddWithValue("villainId", villainId);
                return cmd.ExecuteNonQuery();

            }
        }

        private static void Task5(SqlConnection connection)
        {
            string country = Console.ReadLine();

            string updateTownsSql = @"UPDATE Towns SET [Name] = UPPER([Name]) 
                WHERE CountryCode = (SELECT TOP 1 Id FROM Countries WHERE [Name] = @country)";


            using (SqlCommand cmd = new SqlCommand(updateTownsSql, connection))
            {
                cmd.Parameters.AddWithValue("@country", country);
                int? rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    Console.WriteLine($"{rowsAffected} town names were affected.");

                    List<string> townsAffected = GetTownsByCountry(connection, country);

                    Console.WriteLine($"[ {String.Join(", ", townsAffected)} ]");

                }
            }
        }

        private static List<string> GetTownsByCountry(SqlConnection connection, string country)
        {
            List<string> towns = new List<string>();
            string getTownsByCountrySql = @"SELECT [Name] FROM Towns WHERE CountryCode =
                (SELECT TOP 1 Id FROM Countries WHERE [Name] = @country)";
            using (SqlCommand cmd = new SqlCommand(getTownsByCountrySql, connection))
            {
                cmd.Parameters.AddWithValue("@country", country);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        towns.Add((string)reader["Name"]);
                    }
                }
            }

            return towns;
        }

        private static void Task4(SqlConnection connection)
        {
            string[] tokens = Console.ReadLine().Split();

            string minionName = tokens[1];

            int minionAge = int.Parse(tokens[2]);

            string minionTown = tokens[3];

            string villainName = Console.ReadLine().Split().Skip(1).FirstOrDefault();


            // if town doesn't exist insert it
            int? townId = GetTownByName(connection, minionTown);

            if (townId == null)
            {
                AddTown(connection, minionTown);
                townId = GetTownByName(connection, minionTown);
                Console.WriteLine($"Town {minionTown} was added to the database.");
            }
                            ;

            int? villainId = GetVillainByName(connection, villainName);

            if (villainId == null)
            {
                AddVillain(connection, villainName);

                villainId = GetVillainByName(connection, villainName);

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }

            int? minionId = GetMinionByName(connection, minionName);

            AddMinionVillain(connection, minionId, villainId);

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static void Task3(SqlConnection connection)
        {
            int villainId = int.Parse(Console.ReadLine());

            string villainName = GetVillainNameById(connection, villainId);

            if (villainName == null)
            {
                Console.WriteLine($"No villain with ID {villainId} exists in the database.");
            }
            else
            {
                Console.WriteLine($"Villain: {villainName}");

                GetMinionNames(connection, villainId);
            }
        }

        private static void AddMinionVillain(SqlConnection connection, int? minionId, int? villainId)
        {
            ;
            string commandText = $"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES({minionId}, {villainId})";

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }

        }

        private static int? GetMinionByName(SqlConnection connection, string minionName)
        {
            string commandText = $"SELECT Id FROM Minions WHERE [Name] = @minionName";

            int? result;

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@minionName", minionName);

                result = (int?)command.ExecuteScalar();
            }

            return result;
        }

        private static void AddMinion(SqlConnection connection, string minionName, int minionAge, string minionTown)
        {
            string insertMinionStatement =
                                    $"INSERT INTO Minions ([Name], Age, TownId) VALUES (@minionName, @minionAge, @minionTown)";

            using (SqlCommand command = new SqlCommand())
            {

                command.Connection = connection;

                command.CommandText = insertMinionStatement;

                command.Parameters.AddWithValue("@minionName", minionName);

                command.Parameters.AddWithValue("@minionAge", minionAge);

                command.Parameters.AddWithValue("@minionTown", minionTown);

                command.ExecuteNonQuery();
            }
        }

        private static void AddVillain(SqlConnection connection, string villainName)
        {
            string statement = $"INSERT INTO Villains ([Name], EvilnessFactorId) VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(statement, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                command.ExecuteNonQuery();
            }
        }

        private static int? GetVillainByName(SqlConnection connection, string villainName)
        {
            string commandText = $"SELECT Id FROM Villains WHERE [Name] = @villainName";

            int? result;

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                result = (int?)command.ExecuteScalar();
            }

            return result;
        }

        private static void AddTown(SqlConnection connection, string minionTown)
        {
            string statement = $"INSERT INTO Towns ([Name]) VALUES (@minionTown)";

            using (SqlCommand command = new SqlCommand(statement, connection))
            {
                command.Parameters.AddWithValue("@minionTown", minionTown);
            }
        }

        private static int? GetTownByName(SqlConnection connection, string minionTown)
        {
            string commandText = $"SELECT Id FROM Towns WHERE [Name] = @townName";

            int? result;

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@townName", minionTown);

                result = (int?)command.ExecuteScalar();
            }

            return result;
        }


        private static string GetVillainNameById(SqlConnection connection, int villainId)
        {
            string name;

            string commandText = "SELECT [Name] FROM Villains WHERE Id = @Id";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@Id", villainId);

                name = (string)command.ExecuteScalar();
            }
            return name;
        }

        private static void GetMinionNames(SqlConnection connection, int villainId)
        {
            string commandText =
              @"SELECT m.[Name] 
                FROM Villains AS v
                JOIN MinionsVillains AS mv
	            ON mv.VillainId = v.Id
                JOIN Minions AS m
	                ON m.Age = mv.MinionId
                WHERE v.Id = @Id
                ORDER BY m.[Name]";

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@Id", villainId);
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    int rownNumber = 0;

                    while (reader.Read())
                    {
                        rownNumber++;

                        string minionName = (string)reader["name"];

                        Console.WriteLine($"{rownNumber}. {minionName}");
                    }

                    if (rownNumber == 0)
                    {
                        Console.WriteLine("(no minions)");
                    }
                }

            }

        }

        private static void GetVillainMinionsCount(SqlConnection connection)
        {

            string commandText = @"SELECT 
                v.[Name]
	            , COUNT(mv.MinionId) AS Minions
                FROM Villains AS v
                JOIN MinionsVillains AS mv
                ON v.Id = mv.VillainId
                GROUP BY v.Name
                HAVING COUNT(mv.MinionId) > 2
                ORDER BY Minions DESC";

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string villainName = (string)reader["name"];
                        int minionsCount = (int)reader[1];
                        Console.WriteLine($"{villainName} - {minionsCount}");
                    }
                }

            }
        }

        private static void InsertInitialValuesIntoTables(SqlConnection connection)
        {
            List<string> insertStatements = new List<string>{
            "INSERT INTO Countries([Name]) VALUES('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')",

            "INSERT INTO Towns([Name], CountryCode) VALUES('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)",

            "INSERT INTO Minions(Name, Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)",

            "INSERT INTO EvilnessFactors(Name) VALUES('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')",

            "INSERT INTO Villains(Name, EvilnessFactorId) VALUES('Gru', 2),('Victor', 1),('Jilly', 3),('Miro', 4),('Rosen', 5),('Dimityr', 1),('Dobromir', 2)",

            "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(4, 2),(1, 1),(5, 7),(3, 5),(2, 6),(11, 5),(8, 4),(9, 7),(7, 1),(1, 3),(7, 3),(5, 3),(4, 3),(1, 2),(2, 1),(2, 7)"

            };

            insertStatements.ForEach(c => ExecuteNonQuery(connection, c));
        }


        private static void CreateTables(SqlConnection connection)
        {
            List<string> createStatements = new List<string> {

                "CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",

                "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",

                "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))",

                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",

                "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",

                "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id), VillainId INT FOREIGN KEY REFERENCES Villains(Id), CONSTRAINT PK_MinionsVillains PRIMARY KEY(MinionId, VillainId))"

            };

            createStatements.ForEach(c => ExecuteNonQuery(connection, c));
        }

        private static void UseDatabase(SqlConnection connection, string databaseName)
        {
            string commandText = $"USE {databaseName}";

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                object commandResult = command.ExecuteScalar();
            }
        }

        private static bool CheckDatabaseExists(SqlConnection connection, string databaseName)
        {
            bool result = true;

            string commandText = $"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'";

            using (SqlCommand command = new SqlCommand(commandText, connection))
            {

                object commandResult = command.ExecuteScalar();
                if (commandResult == null)
                {
                    result = false;
                }

            }

            return result;
        }

        private static int ExecuteNonQuery(SqlConnection connection, string commandText)
        {
            int result;

            using (SqlCommand command = new SqlCommand())
            {

                command.Connection = connection;

                command.CommandText = commandText;

                result = command.ExecuteNonQuery();
            }

            return result;
        }
    }
}
