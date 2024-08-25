using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Data;
using System.Globalization;

namespace BettingBot;

//914974

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

        // Начальная сумма ставки
        double baseBet = 1.0;
        double currentBet = baseBet;

        // Получаем начальный баланс
        double initialBalance = GetBalance(driver);

        // Указатель на первую ставку
        bool isFirstBet = true;

        // Бесконечный цикл для выполнения ставок
        while (true)
        {
            // Выполнение ставки и обновление баланса
            initialBalance = PlaceBetWhenPossible(driver, ref currentBet, initialBalance, ref isFirstBet);

            // Небольшая пауза перед следующим циклом
            Thread.Sleep(1000);

            isFirstBet = false; // После первой итерации снимаем флаг первой ставки
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

    private static double PlaceBetWhenPossible(ChromeDriver driver, ref double currentBet, double initialBalance, ref bool isFirstBet)
    {
        // Проверяем, активна ли кнопка
        var betButton = driver.FindElement(By.Id("newBetApiBlue"));

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

        // Кнопка активна, ждем еще 2 секунды для того чтобы баланс точно обновился
        Thread.Sleep(2000);

        // Проверяем новый баланс
        double newBalance = GetBalance(driver);

        // Если это первая ставка, устанавливаем ставку на 1
        if (isFirstBet)
        {
            currentBet = 1.0;
            isFirstBet = false;
            Console.WriteLine("Первая ставка! Устанавливаем ставку на 1.");
        }
        else
        {
            // Проверяем выиграли или проиграли
            if (newBalance > initialBalance)
            {
                // Выигрыш: сбрасываем ставку на минимальную
                currentBet = 1.0;
                Console.WriteLine("Выигрыш! Баланс обновился, возвращаемся к начальной ставке.");
            }
            else
            {
                // Проигрыш: удваиваем ставку
                currentBet *= 2;
                Console.WriteLine("Проигрыш. Удваиваем ставку.");
            }
        }

        // Устанавливаем текущую ставку
        driver.FindElement(By.XPath("//*[@id=\"betAmount\"]")).Clear();
        driver.FindElement(By.XPath("//*[@id=\"betAmount\"]")).SendKeys($"{currentBet:F2}");

        // Нажимаем на кнопку для ставки
        betButton.Click();

        // Небольшая задержка после ставки, чтобы не было множественных нажатий
        Thread.Sleep(1000);

        return newBalance; // Возвращаем текущий баланс после ставки
    }


    private static double GetBalance(ChromeDriver driver)
    {
        // Находим элемент, содержащий баланс, и извлекаем текст
        var balanceText = driver.FindElement(By.Id("updateBalance")).Text;

        // Преобразуем текст баланса в double
        return double.Parse(balanceText, CultureInfo.InvariantCulture);
    }
}