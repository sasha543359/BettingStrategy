using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Data;

namespace BettingBot;

public class Program
{
    public static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    private static async Task MainAsync(string[] args)
    {
        var options = new ChromeOptions();

        var driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl("https://zooma26.casino/");
        Thread.Sleep(2000);

        // Логика авторизации
        SignIn(driver);

        // Переход в зону ставок
        driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div[2]/div[1]/a[2]")).Click();
        Thread.Sleep(3000);

        // Бесконечный цикл для выполнения ставок
        while (true)
        {
            // Выполнение ставки
            PlaceBetWhenPossible(driver, 1);
        }
    }

    private static void SignIn(ChromeDriver driver)
    {
        driver.FindElement(By.ClassName("rightBlockNotAuthTextDown")).Click();
        Thread.Sleep(2000);

        // Через Яндекс
        driver.FindElement(By.XPath("/html/body/div[2]/div[6]/div[3]/a[3]")).Click();
        Thread.Sleep(2000);

        // Другие способы входа
        driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div[2]/div/div/div[2]/div[3]/div/form/div[2]/button")).Click();
        Thread.Sleep(2000);

        // Почта
        var login = "sashaoglashevichi@gmail.com";

        // Поле для вставки почты
        var loginField = driver.FindElement(By.XPath("//*[@id=\"passp-field-login\"]"));
        loginField.SendKeys(login);
        loginField.Click();
        Thread.Sleep(2000);

        // Кнопка войти
        driver.FindElement(By.XPath("//*[@id=\"passp:sign-in\"]")).Click();
        Thread.Sleep(2000);

        // Кнопка отправить письмо для входа
        driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div[2]/div/div/div[2]/div[3]/div/div/form/div/div[3]/div/button")).Click();
        Thread.Sleep(2000);
    }

    private static void PlaceBetWhenPossible(ChromeDriver driver, int betAmount)
    {
        // Проверяем, активна ли кнопка
        var betButton = driver.FindElement(By.Id("newBetApiGray"));

        // Ожидание, пока кнопка станет неактивной (начало вращения)
        while (betButton.GetAttribute("disabled") == null)
        {
            Thread.Sleep(500); // Ждем 500 мс перед проверкой снова
        }

        // Ожидание окончания вращения (кнопка снова станет активной)
        while (betButton.GetAttribute("disabled") != null)
        {
            Thread.Sleep(500); // Ждем 500 мс перед проверкой снова
        }

        // Теперь кнопка активна, можно делать ставку
        driver.FindElement(By.XPath("//*[@id=\"betAmount\"]")).Clear();
        driver.FindElement(By.XPath("//*[@id=\"betAmount\"]")).SendKeys($"{betAmount}");

        // Нажимаем на кнопку для ставки
        betButton.Click();

        // Небольшая задержка после ставки, чтобы не было множественных нажатий
        Thread.Sleep(1000);
    }
}