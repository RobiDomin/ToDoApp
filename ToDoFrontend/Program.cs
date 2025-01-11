using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ToDoFrontend
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5000/");

            Console.WriteLine("=== To-Do App ===");
            Console.WriteLine("1. Wyświetl zadania");
            Console.WriteLine("2. Dodaj zadanie");
            Console.WriteLine("3. Oznacz zadanie jako ukończone");
            Console.WriteLine("4. Usuń zadanie");
            Console.WriteLine("0. Wyjście");

            while (true)
            {
                Console.Write("Wybierz opcję: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await GetTasks(client);
                        break;
                    case "2":
                        await AddTask(client);
                        break;
                    case "3":
                        await CompleteTask(client);
                        break;
                    case "4":
                        await DeleteTask(client);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja!");
                        break;
                }
            }
        }

        static async Task GetTasks(HttpClient client)
        {
            var tasks = await client.GetFromJsonAsync<ToDoItem[]>("api/todo");
            Console.WriteLine("Twoje zadania:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"[{task.Id}] {task.Title} - {(task.IsCompleted ? "Ukończone" : "Nieukończone")}");
            }
        }

        static async Task AddTask(HttpClient client)
        {
            Console.Write("Podaj tytuł zadania: ");
            var title = Console.ReadLine();
            Console.Write("Podaj opis zadania: ");
            var description = Console.ReadLine();

            var newTask = new ToDoItem { Title = title, Description = description };
            var response = await client.PostAsJsonAsync("api/todo", newTask);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Zadanie dodane pomyślnie!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Błąd podczas dodawania zadania: {response.StatusCode} - {error}");
            }
        }

        static async Task CompleteTask(HttpClient client)
        {
            Console.Write("Podaj ID zadania do ukończenia: ");
            var id = int.Parse(Console.ReadLine());

            var task = new ToDoItem { Id = id, IsCompleted = true }; // Przygotuj zmodyfikowany obiekt
            var response = await client.PutAsJsonAsync($"api/todo/{id}", task);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Zadanie oznaczone jako ukończone!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Błąd podczas aktualizacji zadania: {response.StatusCode} - {error}");
            }
        }

        static async Task DeleteTask(HttpClient client)
        {
            Console.Write("Podaj ID zadania do usunięcia: ");
            var id = int.Parse(Console.ReadLine());
            var response = await client.DeleteAsync($"api/todo/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Zadanie usunięte!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Błąd podczas usuwania zadania: {response.StatusCode} - {error}");
            }
        }
    }

    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
