using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using GameSounds;

public class GameMenu
{
    private const int FieldRows = 25;
    private const int FieldCols = 100;
    private const char arrowChar = '>';

    private static int musicPower = 10;
    private static int gameEffectsPower = 10;
    
    static Tuple<int, int> startGameCoordinates = new Tuple<int, int>(45, 12);
    static Tuple<int, int> settingsCoordinates = new Tuple<int,int>(46, 14);
    static Tuple<int, int> exitCoordinates = new Tuple<int,int>(48, 16);
    static Tuple<int, int> settingsHeaderCoordinates = new Tuple<int, int>(22, 2); 
    static Tuple<int, int> musicVolumeCoordinates = new Tuple<int, int>(30, 12);
    static Tuple<int, int> gameEffectsVolumeCoordinates = new Tuple<int, int>(23, 14);
    static Tuple<int, int> backCoordinates = new Tuple<int, int>(46, 17);
    static Tuple<int, int> musicLowCoordinates = new Tuple<int, int>(46, 12); 
    static Tuple<int, int> musicMediumCoordinates = new Tuple<int, int>(54, 12);
    static Tuple<int, int> musicHighCoordinates = new Tuple<int, int>(64, 12); 
    static Tuple<int, int> gameEffectsLowCoordinates = new Tuple<int, int>(46, 14);
    static Tuple<int, int> gameEffectsMediumCoordinates = new Tuple<int, int>(54, 14);
    static Tuple<int, int> gameEffectsHighCoordinates = new Tuple<int, int>(64, 14);

    static Sounds sounds = new Sounds(); // Load sounds

    public static void MainMenu()
    {
        Console.Title = "Space Defenders"; // Change title
        Console.SetWindowSize(FieldCols + 2, FieldRows + 3); // Set console width and height
        Console.SetBufferSize(FieldCols + 2, FieldRows + 3); // Setbuffer size
        Console.BackgroundColor = ConsoleColor.White; // Set background color
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Black; // Printing with black color
        Console.CursorVisible = false; // Hide curson

        sounds.PlayMenuSoundtrack();

        string logoLine = String.Empty; // Read logo
        List<string> logo = new List<string>();
        StreamReader logoReader = new StreamReader("logo-2.txt", Encoding.UTF8);
        using (logoReader)
        {
            logoLine = logoReader.ReadLine();
            while (logoLine != null)
            {
                logo.Add(logoLine);
                logoLine = logoReader.ReadLine();
            }
        }
        Console.WriteLine();
        for (int i = 0; i < logo.Count; i++)
        {
            Console.SetCursorPosition(0, i + 2);
            Console.Write(logo[i]);
        }

        string currentMenuOption = "Start Game";
        Console.SetCursorPosition(startGameCoordinates.Item1, startGameCoordinates.Item2);
        Console.WriteLine("Start Game");
        Console.SetCursorPosition(settingsCoordinates.Item1, settingsCoordinates.Item2);
        Console.WriteLine("Settings");
        Console.SetCursorPosition(exitCoordinates.Item1, exitCoordinates.Item2);
        Console.WriteLine("Exit");

        Console.SetCursorPosition(startGameCoordinates.Item1 - 1, startGameCoordinates.Item2);
        Console.Write(arrowChar);
        ConsoleKeyInfo presedKey = Console.ReadKey(true);
        while (presedKey.Key != ConsoleKey.Enter && presedKey.Key != ConsoleKey.Spacebar)
        {
            switch (presedKey.Key)
            {
                case ConsoleKey.DownArrow:
                    if (currentMenuOption.Equals("Start Game"))
                    {
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(startGameCoordinates.Item1 - 1, startGameCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Settings";
                    }
                    else if (currentMenuOption.Equals("Settings"))
                    {
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(exitCoordinates.Item1 - 1, exitCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Exit";
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (currentMenuOption.Equals("Exit"))
                    {
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(exitCoordinates.Item1 - 1, exitCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Settings";
                    }
                    else if (currentMenuOption.Equals("Settings"))
                    {
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(settingsCoordinates.Item1 - 1, settingsCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(startGameCoordinates.Item1 - 1, startGameCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentMenuOption = "Start Game";
                    }
                    break;
                case ConsoleKey.Escape:
                    Console.SetCursorPosition(FieldCols - 1, FieldRows - 1);
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

            presedKey = Console.ReadKey(true);
        }

        if (currentMenuOption.Equals("Start Game"))
        {
            sounds.StopMenuSoundtrack();
            sounds.PlayStartGameSound();
            Thread.Sleep(1500);
            Console.SetCursorPosition(0, 0);
            GameEngine.MainEngine();
        }
        else if (currentMenuOption.Equals("Settings"))
        {
            sounds.StopMenuSoundtrack();
            sounds.PlayMenuSound();
            Thread.Sleep(200);
            
            sounds.PlayMenuSoundtrack();
            Settings("Music volume");
        }
        else if (currentMenuOption.Equals("Exit"))
        {
            sounds.StopMenuSoundtrack();
            sounds.PlayMenuSound();
            Thread.Sleep(300);
            Environment.Exit(0);
        }
    }

    public static void Settings(string currentSettingsOption)
    {
        Console.Clear();
        StreamReader settingsHeaderReader = new StreamReader("settingsHeader.txt");
        List<string> settingsHeader = new List<string>();
        using (settingsHeaderReader)
        {
            string currentLine = settingsHeaderReader.ReadLine();
            while (currentLine != null)
            {
                settingsHeader.Add(currentLine);
                currentLine = settingsHeaderReader.ReadLine();
            }
        }
        for (int i = 0; i < settingsHeader.Count; i++)
        {
            Console.SetCursorPosition(settingsHeaderCoordinates.Item1, settingsHeaderCoordinates.Item2 + i);
            Console.Write(settingsHeader[i]);
        }

        sounds.PlayMenuSoundtrack();

        Console.SetCursorPosition(musicVolumeCoordinates.Item1, musicVolumeCoordinates.Item2);
        Console.Write("Music volume:   Low     Medium    High");
        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1, gameEffectsVolumeCoordinates.Item2);
        Console.Write("Game effects volume:   Low     Medium    High");
        Console.SetCursorPosition(backCoordinates.Item1, backCoordinates.Item2);
        Console.Write("Back");

        if (currentSettingsOption.Equals("Music volume"))
        {
            Console.SetCursorPosition(musicVolumeCoordinates.Item1 - 1, musicVolumeCoordinates.Item2);
        }
        else if (currentSettingsOption.Equals("Game effects volume"))
        {
            Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1, gameEffectsVolumeCoordinates.Item2);
        }
        Console.Write(arrowChar);
        ConsoleKeyInfo settingsPressedKey = Console.ReadKey(true);
        while (settingsPressedKey.Key != ConsoleKey.Enter && settingsPressedKey.Key != ConsoleKey.Spacebar)
        {
            switch (settingsPressedKey.Key)
            {
                case ConsoleKey.DownArrow:
                    if (currentSettingsOption.Equals("Music volume"))
                    {
                        sounds.StopMenuSoundtrack();
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(musicVolumeCoordinates.Item1 - 1, musicVolumeCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Game effects volume";
                    }
                    else if (currentSettingsOption.Equals("Game effects volume"))
                    {
                        sounds.StopMenuSoundtrack();
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(backCoordinates.Item1 - 1, backCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Back";
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (currentSettingsOption.Equals("Back"))
                    {
                        sounds.StopMenuSoundtrack();
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(backCoordinates.Item1 - 1, backCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Game effects volume";
                    }
                    else if (currentSettingsOption.Equals("Game effects volume"))
                    {
                        sounds.StopMenuSoundtrack();
                        sounds.PlayMenuSound();
                        Thread.Sleep(200);
                        sounds.PlayMenuSoundtrack();
                        Console.SetCursorPosition(gameEffectsVolumeCoordinates.Item1 - 1,
                            gameEffectsVolumeCoordinates.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(musicVolumeCoordinates.Item1 - 1, musicVolumeCoordinates.Item2);
                        Console.Write(arrowChar);
                        currentSettingsOption = "Music volume";
                    }
                    break;
                default:
                    break;
            }

            settingsPressedKey = Console.ReadKey(true);
        }

        if (currentSettingsOption.Equals("Music volume"))
        {
            sounds.StopMenuSoundtrack();
            sounds.PlayMenuSound();
            Thread.Sleep(200);
            sounds.PlayMenuSoundtrack();
            string currentMusicOption = "Low";
            Console.SetCursorPosition(musicLowCoordinates.Item1 - 1, musicLowCoordinates.Item2);
            Console.Write(arrowChar);
            ConsoleKeyInfo musicPressedKey = Console.ReadKey(true);
            while (musicPressedKey.Key != ConsoleKey.Enter && musicPressedKey.Key != ConsoleKey.Spacebar && musicPressedKey.Key != ConsoleKey.Escape)
            {
                switch (musicPressedKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (currentMusicOption.Equals("Low"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(musicLowCoordinates.Item1 - 1, musicLowCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "Medium";
                        }
                        else if (currentMusicOption.Equals("Medium"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicHighCoordinates.Item1 - 1, musicHighCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "High";
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentMusicOption.Equals("High"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(musicHighCoordinates.Item1 - 1, musicHighCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "Medium";
                        }
                        else if (currentMusicOption.Equals("Medium"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(musicMediumCoordinates.Item1 - 1, musicMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(musicLowCoordinates.Item1 - 1, musicLowCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentMusicOption = "Low";
                        }
                        break;
                    case ConsoleKey.Escape:
                        Settings("Music volume");
                        break;
                    default:
                        break;
                }

                musicPressedKey = Console.ReadKey(true);
            }

            if (currentMusicOption.Equals("Low"))
            {
                musicPower = 10;
            }
            else if (currentMusicOption.Equals("Medium"))
            {
                musicPower = 30;
            }
            else if (currentMusicOption.Equals("High"))
            {
                musicPower = 50;
            }
            Settings(currentSettingsOption);
        }
        else if (currentSettingsOption.Equals("Game effects volume"))
        {
            sounds.StopMenuSoundtrack();
            sounds.PlayMenuSound();
            Thread.Sleep(200);
            sounds.PlayMenuSoundtrack();
            string currentGameEffectsOption = "Low";
            Console.SetCursorPosition(gameEffectsLowCoordinates.Item1 - 1, gameEffectsLowCoordinates.Item2);
            Console.Write(arrowChar);
            ConsoleKeyInfo gameEffectsPressedKey = Console.ReadKey(true);
            while (gameEffectsPressedKey.Key != ConsoleKey.Enter && gameEffectsPressedKey.Key != ConsoleKey.Spacebar)
            {
                switch (gameEffectsPressedKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (currentGameEffectsOption.Equals("Low"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(gameEffectsLowCoordinates.Item1 - 1,
                                gameEffectsLowCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "Medium";
                        }
                        else if (currentGameEffectsOption.Equals("Medium"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsHighCoordinates.Item1 - 1,
                                gameEffectsHighCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "High";
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentGameEffectsOption.Equals("High"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(gameEffectsHighCoordinates.Item1 - 1,
                                gameEffectsHighCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "Medium";
                        }
                        else if (currentGameEffectsOption.Equals("Medium"))
                        {
                            sounds.StopMenuSoundtrack();
                            sounds.PlayMenuSound();
                            Thread.Sleep(200);
                            sounds.PlayMenuSoundtrack();
                            Console.SetCursorPosition(gameEffectsMediumCoordinates.Item1 - 1,
                                gameEffectsMediumCoordinates.Item2);
                            Console.Write(' ');
                            Console.SetCursorPosition(gameEffectsLowCoordinates.Item1 - 1,
                                gameEffectsLowCoordinates.Item2);
                            Console.Write(arrowChar);
                            currentGameEffectsOption = "Low";
                        }
                        break;
                    case ConsoleKey.Escape:
                        Settings("Game effects volume");
                        break;
                    default:
                        break;
                }

                gameEffectsPressedKey = Console.ReadKey(true);
            }

            if (currentGameEffectsOption.Equals("Low"))
            {
                gameEffectsPower = 10;
            }
            else if (currentGameEffectsOption.Equals("Medium"))
            {
                gameEffectsPower = 30;
            }
            else if (currentGameEffectsOption.Equals("High"))
            {
                gameEffectsPower = 50;
            }
            Settings(currentSettingsOption);
        }
        else if (currentSettingsOption.Equals("Back"))
        {
            sounds.StopMenuSoundtrack();
            sounds.PlayMenuSound();
            Thread.Sleep(200);
            sounds.PlayMenuSoundtrack();
            MainMenu();
        }
    }
}

