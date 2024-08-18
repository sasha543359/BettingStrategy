namespace BettingStrategy;

public class Program
{
    static int balance = 1000;
    static int stavka = 1;
    static int wins = 0;
    static void Main(string[] args)
    {
        //            "PINK",
        //            "BLUE",
        //            "GRAY",
        //            "ORANGE",

        List<string> colors = new List<string>()
        {

            "PINK","BLUE","GRAY","BLUE","BLUE","BLUE","GRAY","PINK","BLUE","BLUE","PINK", // 912724
            "BLUE","GRAY","BLUE","BLUE","BLUE","GRAY","PINK","BLUE","BLUE","PINK","PINK", // 912735
            "PINK","PINK","BLUE","GRAY","GRAY","BLUE","BLUE","GRAY","GRAY","GRAY","BLUE", // 912746
            "BLUE","BLUE","GRAY","PINK","ORANGE","BLUE","PINK","GRAY","BLUE","GRAY","GRAY", // 912757
            "GRAY","GRAY","BLUE","BLUE","GRAY","PINK","GRAY","GRAY","BLUE","ORANGE","GRAY", // 912768
            "GRAY","BLUE","GRAY","GRAY","GRAY","BLUE","BLUE","GRAY","GRAY","BLUE","GRAY", // 912779
            "BLUE","GRAY","BLUE","GRAY","PINK","GRAY","GRAY","BLUE","GRAY","PINK","GRAY", // 912790
            "GRAY","GRAY","GRAY","PINK","GRAY","BLUE","BLUE","PINK","BLUE","PINK","GRAY", // 912801
            "PINK","PINK","PINK","GRAY","PINK","BLUE","BLUE","BLUE","GRAY","GRAY","GRAY", // 912812
            "BLUE","BLUE","BLUE","ORANGE","GRAY","GRAY","BLUE","BLUE","GRAY","GRAY","PINK", // 912823
            "ORANGE","PINK","GRAY","BLUE","GRAY","BLUE","BLUE","PINK","PINK","BLUE","BLUE", // 912834
            "GRAY","GRAY","BLUE","GRAY","PINK","BLUE","BLUE","PINK","BLUE","GRAY","BLUE", // 912845
            "BLUE","GRAY","BLUE","GRAY","PINK","GRAY","BLUE","BLUE","BLUE","BLUE","BLUE", // 912856
            "GRAY","BLUE","PINK","GRAY","GRAY","BLUE","BLUE","PINK","BLUE","GRAY","GRAY", // 912867
            "PINK","BLUE","BLUE","ORANGE","PINK","PINK","BLUE","GRAY","PINK","BLUE","BLUE", // 912878

        };

        //var colorCounts = colors
        //   .GroupBy(c => c)
        //   .Select(group => new { Color = group.Key, Count = group.Count() });

        //foreach (var color in colorCounts)
        //{
        //    Console.WriteLine($"Цвет '{color.Color}' встречается {color.Count} раз(а)");
        //}

        int id = 1;
        foreach (string color in colors)
        {
            if (balance > 0)
                Console.WriteLine($"Id: {id} Balance: {balance} Stavka: {stavka} {Stavka(color)}");
            else
                return;

            id++;
        }

        Console.WriteLine($"Final balance {balance}");
        Console.WriteLine($"Total wins: {wins}");
    }

    static string Stavka(string color)
    {
        if (color == "GRAY")
        {
            wins++;
            balance += stavka * 2;
            int win = stavka;
            stavka = 1;
            return $"| Win: {win * 2}";
        }
        else
        {
            balance -= stavka;
            int lose = stavka;
            stavka *= 2;
            return $"| Lose: {lose}";
        }
    }
}