using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace KometaRegistryBot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var options = new ChromeOptions();
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");

            // Отключение автоматизации
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);

            // Добавление дополнительных аргументов
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            var gmailAccounts = new List<string> { "email1@gmail.com", "email2@gmail.com", /* ... */ };

            using var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            foreach (var email in gmailAccounts)
            {
                try
                {
                    driver.Navigate().GoToUrl("https://stars-flight.com/s61d44d6c");

                    // Попытка скрыть факт использования Selenium
                    js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");

                    // Ожидание появления поля email
                    wait.Until(drv => drv.FindElement(By.Name("email")));

                    // Пример ввода данных в поле email
                    var emailField = driver.FindElement(By.Name("email"));
                    emailField.Clear();
                    emailField.SendKeys(email);

                    // Добавьте остальные шаги регистрации здесь
                    // Например, ввод пароля, нажатие кнопки подтверждения и т.д.

                    // Задержка для завершения процесса
                    await Task.Delay(5000);

                    // Очистка cookies и кэша перед следующей итерацией
                    driver.Manage().Cookies.DeleteAllCookies();
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine($"Элемент не найден для {email}: {ex.Message}");
                }
                catch (WebDriverException ex)
                {
                    Console.WriteLine($"Ошибка WebDriver при работе с {email}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Неизвестная ошибка при работе с {email}: {ex.Message}");
                }
            }

            // Закрываем браузер после завершения всех операций
            driver.Quit();
        }
    }
}
