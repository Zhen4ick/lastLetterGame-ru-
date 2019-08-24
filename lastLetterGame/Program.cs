using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
namespace Cities
{
    class Program
    {

        static List<string> cities = new List<string>();

        static void loadCities()
        {
            XmlDocument xml = new XmlDocument(); //XML файл с городами
            XmlElement xRoot;
            xml.Load("rocid.xml");
            xRoot = xml.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "name" && Regex.IsMatch(childnode.InnerText, "[а-я]+"))
                    {
                        cities.Add(childnode.InnerText);
                    }
                }
            }
        }

        static void sortCities()
        {
            //Сортировка массива
            cities.Sort();

            //Удаление из городов из списка, имеющие в своем имени латинские буквы

            for (int i = 0; i < cities.Count; i++)
            {
                if (cities[i].ToString()[0] != 'А')
                {
                    cities.Remove(cities[i]);
                }
                else
                {
                    break;
                }
            }
        }

        static void Main(string[] args)
        {

            User user = new User();
            Bot bot = new Bot();
            loadCities();
            sortCities();

            //Вступление
            Console.WriteLine("Привет.\nДавай поиграем в города.\nВы начинаете.");
            while (true)
            {
                Console.Write("Вы: ");
                user.choice = Console.ReadLine();
                if (cities.Contains(user.choice) &&
                    user.used_cities.Contains(user.choice) == false &&
                    bot.used_cities.Contains(user.choice) == false)
                {
                    user.used_cities.Add(user.choice);
                    cities.Remove(user.choice);
                    char lastSymbol = new char();

                    if (user.choice[user.choice.Length - 1] == 'ь' || user.choice[user.choice.Length - 1] == 'ъ')
                    {
                        lastSymbol = user.choice.ToUpper()[user.choice.Length - 2];
                    }
                    else
                    {
                        lastSymbol = user.choice.ToUpper()[user.choice.Length - 1];
                    }

                    try
                    {
                        bot.choice = cities[bot.findIndexOfCity(cities, lastSymbol)];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        bot.Say("Игра окончена. Все города использованы");
                        Console.ReadKey();
                        break;
                    }

                    if (bot.findIndexOfCity(cities, lastSymbol) == 0 && lastSymbol != cities[0].ToString()[0])
                    {
                        bot.Say("Я проиграл :(. Вы начинаете.");
                        user.used_cities.Add(user.choice);
                        continue;
                    }
                    bot.Say(bot.choice);
                    bot.used_cities.Add(bot.choice);
                    cities.Remove(bot.choice);
                }
                else if (user.choice == "exit")
                {
                    break;
                }
                else if (user.used_cities.Contains(user.choice))
                {
                    bot.Say("Вы уже использовали этот город");
                }
                else if (bot.used_cities.Contains(user.choice))
                {
                    bot.Say("Я уже использовал этот город");
                }
                else
                {
                    bot.Say("Я не знаю такого города");
                }
            }
        }
    }
    class User
    {
        public List<string> used_cities = new List<string>();
        public string choice;
        public int findIndexOfCity(List<string> array, char value)
        {
            int low = 0;
            int high = array.Count - 1;
            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (array[mid].ToString()[0].CompareTo(value) == 0)
                {
                    return mid;
                }
                else if (value.CompareTo(array[mid].ToString()[0]) > 0)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }
            return 0;
        }
    }
    class Bot : User
    {
        public void Say(string text)
        {
            Console.WriteLine("Бот: " + text);
        }
    }
}
