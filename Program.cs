using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UserRegistrationClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Адрес вашего сервера
            string url = "https://localhost:7282/api/Registration"; // Используйте ваш фактический порт

            // Создаем нового пользователя
            var newUser = new User
            {
                Username = "testuser3",
                Password = "password123",
                Email = "testuser@example.com"
            };

            // Сериализуем пользователя в JSON
            string jsonData = JsonConvert.SerializeObject(newUser);

            // Создаем HttpClient с обработчиком, игнорирующим ошибки SSL-сертификата
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (var client = new HttpClient(handler))
            {
                // Настраиваем запрос
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    // Отправляем POST-запрос
                    var response = await client.PostAsync(url, content);

                    // Читаем ответ
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Пользователь успешно зарегистрирован.");
                        Console.WriteLine("Ответ сервера: " + responseString);
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка регистрации. Статус: {response.StatusCode}");
                        Console.WriteLine("Ответ сервера: " + responseString);
                    }
                }
                catch (HttpRequestException hre)
                {
                    Console.WriteLine("Ошибка HTTP-запроса: " + hre.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                }
            }
        }

        // Модель пользователя должна совпадать с моделью на сервере
        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
    }
}
