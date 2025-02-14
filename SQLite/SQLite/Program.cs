using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SQLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cash cash = new Cash();
            cash.Start();
            InitializeDatabase();

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Просмотреть все товары");
                Console.WriteLine("3. Обновить товар");
                Console.WriteLine("4. Удалить товар");
                Console.WriteLine("5. Выйти");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        ViewProducts();
                        break;
                    case "3":
                        UpdateProduct();
                        break;
                    case "4":
                        DeleteProduct();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
        static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection("Data Source=products.db;Version=3;"))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Price REAL NOT NULL,
                        Quantity INTEGER NOT NULL
                    );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        static void AddProduct()
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            Console.Write("Введите цену товара: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Введите количество товара: ");
            int quantity = int.Parse(Console.ReadLine());

            using (var connection = new SQLiteConnection("Data Source=products.db;Version=3;"))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Products (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Товар успешно добавлен!");
        }
        static void ViewProducts()
        {
            using (var connection = new SQLiteConnection("Data Source=products.db;Version=3;"))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Products";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Список товаров:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, Название: {reader["Name"]}, Цена: {reader["Price"]}, Количество: {reader["Quantity"]}");
                        }
                    }
                }
            }
        }
        static void UpdateProduct()
        {
            Console.Write("Введите ID товара для обновления: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Введите новое название товара: ");
            string name = Console.ReadLine();
            Console.Write("Введите новую цену товара: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Введите новое количество товара: ");
            int quantity = int.Parse(Console.ReadLine());

            using (var connection = new SQLiteConnection("Data Source=products.db;Version=3;"))
            {
                connection.Open();
                string updateQuery = "UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity WHERE Id = @Id";
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine("Товар успешно обновлен!");
                    else
                        Console.WriteLine("Товар с указанным ID не найден.");
                }
            }
        }
        static void DeleteProduct()
        {
            Console.Write("Введите ID товара для удаления: ");
            int id = int.Parse(Console.ReadLine());

            using (var connection = new SQLiteConnection("Data Source=products.db;Version=3;"))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Products WHERE Id = @Id";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine("Товар успешно удален!");
                    else
                        Console.WriteLine("Товар с указанным ID не найден.");
                }
            }
        }
    }
}
