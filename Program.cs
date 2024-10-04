using Microsoft.Win32;

namespace HW_Registry
{
     class Program
    {

        
        static void Main(string[] args)
        {
            
            const string keyPath = @"Software\HW_Registry";
            Menu(keyPath);
            
            string defaultUserName = "Guest";
            int defaultFontSize = 12;
           
            string userName = ReadRegistryValue(keyPath, "UserName", defaultUserName);
            int fontSize = ReadRegistryValue(keyPath, "FontSize", defaultFontSize);

            Console.WriteLine("Текущие настройки:");
            Console.WriteLine("Имя пользователя: " + userName);
            Console.WriteLine("Размер шрифта: " + fontSize);
            
            userName = "John";
            fontSize = 16;

            WriteRegistryValue(keyPath, "UserName", userName);
            WriteRegistryValue(keyPath, "FontSize", fontSize);

            Console.WriteLine("\nНастройки успешно обновлены.");
        }

        static void Menu(string keyPath)
        {
            string[] menuItems = new string[] { "посмотреть текущие", "изменить", "Выход" };
            int index = 0;
            Console.WriteLine("Меню\n");
            

          
            Console.Clear();

            
            while (true)
            {
                DrawMenu(menuItems,index );
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (index < menuItems.Length - 1)
                            index++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                            index--;
                        break;
                    case ConsoleKey.Enter:
                        switch (index)
                        {
                            case 0: 
                                ShowCurrentSettings(keyPath);
                                 break;
                            case 1: 
                                ChangeSettings(keyPath); 
                                break;
                            case 2:
                                Console.WriteLine("Выбран выход из приложения");
                                return;
                            default:
                                Console.WriteLine($"Выбран пункт {menuItems[index]}");
                                break;
                        }
                        break;
                }
            }
            static void DrawMenu(string[] items, int index)
            {
                Console.Clear();
                for (int i = 0; i < items.Length; i++)
                {
                    if (i == index)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(items[i]);
                    Console.ResetColor();
                }
            }
        }
        static void ShowCurrentSettings(string keyPath)
        {
            string userName = ReadRegistryValue(keyPath, "UserName", "Guest");
            int fontSize = ReadRegistryValue(keyPath, "FontSize", 12);

            Console.WriteLine($"Текущие настройки:");
            Console.WriteLine($"Имя пользователя: {userName}");
            Console.WriteLine($"Размер шрифта: {fontSize}");
            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        static void ChangeSettings(string keyPath)
        {
            Console.Write("Введите новое имя пользователя: ");
            string newUserName = Console.ReadLine();

            Console.Write("Введите новый размер шрифта: ");
            if (int.TryParse(Console.ReadLine(), out int newFontSize))
            {
                WriteRegistryValue(keyPath, "UserName", newUserName);
                WriteRegistryValue(keyPath, "FontSize", newFontSize);
                Console.WriteLine("Настройки обновлены.");
            }
            else
            {
                Console.WriteLine("Некорректный ввод.");
            }

            Console.WriteLine("Нажмите любую клавишу");
            Console.ReadKey();
        }

        static T ReadRegistryValue<T>(string keyPath, string valueName, T defaultValue)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath))
            {
                if (key != null)
                {
                    object value = key.GetValue(valueName);
                    if (value != null)
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
            }
            return defaultValue;
        }

        static void WriteRegistryValue<T>(string keyPath, string valueName, T value)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key != null)
                {
                    key.SetValue(valueName, value);
                }
            }
        }
    }
    
}
