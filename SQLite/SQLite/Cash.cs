using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SQLite
{

    internal class Cash
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить данные в кэш");
                Console.WriteLine("2. Получить данные из кэша");
                Console.WriteLine("3. Удалить данные из кэша");
                Console.WriteLine("4. Очистить кэш");
                Console.WriteLine("5. Выйти");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddToCache();
                        break;
                    case "2":
                        GetFromCache();
                        break;
                    case "3":
                        RemoveFromCache();
                        break;
                    case "4":
                        ClearCache();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
        void AddToCache()
        {
            Console.Write("Введите ключ: ");
            string key = Console.ReadLine();
            Console.Write("Введите значение: ");
            string value = Console.ReadLine();
            Console.Write("Введите время жизни кэша (в секундах): ");
            int duration = int.Parse(Console.ReadLine());


            Cache.Add(key, value, DateTimeOffset.Now.AddSeconds(duration));
            Console.WriteLine("Данные успешно добавлены в кэш!");
        }

        void GetFromCache()
        {
            Console.Write("Введите ключ: ");
            string key = Console.ReadLine();

            if (Cache.Get(key) is string cachedValue)
            {
                Console.WriteLine($"Значение для ключа '{key}': {cachedValue}");
            }
            else
            {
                Console.WriteLine($"Данные для ключа '{key}' не найдены в кэше.");
            }
        }

        static void RemoveFromCache()
        {
            Console.Write("Введите ключ: ");
            string key = Console.ReadLine();

            if (Cache.Remove(key) != null)
            {
                Console.WriteLine($"Данные для ключа '{key}' успешно удалены из кэша.");
            }
            else
            {
                Console.WriteLine($"Данные для ключа '{key}' не найдены в кэше.");
            }
        }

        static void ClearCache()
        {

            foreach (var item in Cache)
            {
                Cache.Remove(item.Key);
            }
            Console.WriteLine("Кэш успешно очищен.");
        }

    }
}
