using System;

public class Introduction
{
    public static void Intro(string myText)
    {
        for (int i = 0; i < myText.Length; i++)
        {
            Console.Write(myText[i]);
            System.Threading.Thread.Sleep(100);
        }
        Console.WriteLine();
    }
    public static void FadeIn(string myText)
    {
        Console.SetCursorPosition(15, 12);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(myText);
        System.Threading.Thread.Sleep(750);
        Console.Clear();
        Console.SetCursorPosition(15, 12);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(myText);
        System.Threading.Thread.Sleep(1500);
        Console.Clear();
    }

    private static void FadeOut(string myText)
    {
        Console.SetCursorPosition(15, 10);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(myText);
        System.Threading.Thread.Sleep(1000);
        Console.Clear();
        Console.SetCursorPosition(15, 10);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(myText);
        System.Threading.Thread.Sleep(1000);
        Console.Clear();
        Console.SetCursorPosition(15, 10);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(myText);
        System.Threading.Thread.Sleep(1000);
        Console.Clear();

    }


    public static void GameIntroduction()
    {
        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"typewriter.wav");
        player.Load();
        player.PlayLooping();
        Intro("Year: 2738");
        Intro("System: Solar");
        Intro("Planet: Earth");
        player.Stop();
        System.Threading.Thread.Sleep(2000);
        Console.Clear();
        player.PlayLooping();
        Intro("      Gaming has been banned by all" + System.Environment.NewLine +
                "         Governments across the planet" +
                System.Environment.NewLine + "          In order to survive mankind has to go back" +
                System.Environment.NewLine + "           To writing shitty console games " +
                System.Environment.NewLine + "             With the crappiest intros ever," +
                System.Environment.NewLine + "              Because they have run out of ideas and time");                                                                                                  //mrazim BMW
        player.Stop();
        System.Threading.Thread.Sleep(2000);
        Console.Clear();
        System.Media.SoundPlayer player2 = new System.Media.SoundPlayer(@"realshit.wav");
        player2.Play();
        FadeOut("Team HYPORI proudly-ish presents");
        System.Threading.Thread.Sleep(750);
        FadeIn("Space Defenders");
        player2.Stop();
        //while (!Console.KeyAvailable)
        //{
        //    FadeOut("Press any key to continue...");
        //}
    }
}

