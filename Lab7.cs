using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lab7//validates standard US data entries
{
    class Program
    {
        static void Main(string[] args)
        {
            UserInput();
        }

        static void Header(string ID)
        {
            Console.Write("\nPlease enter a valid {0}:  ", ID);
        }

        static void UserInput()
        {
            bool valid = false;
            bool retry = true;
            while (retry)
            {
                List<KeyValuePair<string, Action>> uIr = new List<KeyValuePair<string, Action>>();//user input request
                uIr.Add(new KeyValuePair<string, Action>("Name", () => ValName(ref valid)));
                uIr.Add(new KeyValuePair<string, Action>("Email", () => ValEmail(ref valid)));
                uIr.Add(new KeyValuePair<string, Action>("Phone", () => ValPhone(ref valid)));
                uIr.Add(new KeyValuePair<string, Action>("Date", () => ValDate(ref valid)));

                for (int i = 0; i < uIr.Count; i++)
                {
                    valid = false;
                    while (!valid)
                    {
                        Header(uIr[i].Key); 
                        uIr[i].Value.Invoke();
                        Valid(uIr[i].Key, valid);
                    }
                }
                retry = Retry();
            }
            Exit();
        }

        static void ValName(ref bool valid)
        {
            Regex alpha = new Regex(@"([A-Z]{1,})([A-z]{1,})");
            Regex apostrophe = new Regex(@"([A-Z]{1,})\'([A-z]{1,})");
            string fullName = Console.ReadLine();
            if (!valid && Null(fullName) && fullName.Length <= 30)
            {
                string[] names = fullName.Split(' ');
                foreach (string name in names)
                {
                    if (alpha.IsMatch(name))
                    {
                        valid = true;
                    }
                }
            }
        }

        static void ValEmail(ref bool valid)
        {
            string input = Console.ReadLine();
            valid = false;
            if (!valid && Null(input))
            {
                string[] split1 = input.Split('@');
                string mailbox = split1[0];
                Regex email = new Regex(@"([A-z0-9]{5,30})");
                if (split1.Length == 2 && email.IsMatch(mailbox))
                {
                    string[] split2 = split1[1].Split('.');
                    string domain = split2[0];
                    email = new Regex(@"([A-z0-9]{5,10})");
                    if (split2.Length == 2 && email.IsMatch(domain))
                    {
                        string domainExt = split2[1];
                        email = new Regex(@"([A-z0-9]{2,3})");
                        if (email.IsMatch(domainExt))
                        {
                            valid = true;
                        }
                    }
                }
            }
        }

        static void ValPhone(ref bool valid)
        {
            string input = Console.ReadLine();
            valid = false;
            if (!valid && Null(input))
            {
                string[] split = input.Split('-');
                if (split.Length == 3)
                {
                    string area = split[0];
                    string prefix = split[1];
                    string line = split[2];
                    Regex num = new Regex(@"\d+");
                    if (num.IsMatch(prefix) && prefix.Length == 3)
                    {
                        if (num.IsMatch(area) && area.Length == 3)
                        {
                            if (num.IsMatch(line) && line.Length == 4)
                            {
                                valid = true;
                            }
                        }
                    }
                }
            }
        }

        static void ValDate(ref bool valid)
        {
            string input = Console.ReadLine();
            valid = false;
            if (!valid && Null(input))
            {
                string[] split = input.Split('/');
                if (split.Length == 3)
                {
                    string day = split[0];
                    string month = split[1];
                    string year = split[2];
                    Regex num = new Regex(@"\d+");
                    if (num.IsMatch(month) && month.Length == 2)
                    {
                        if (num.IsMatch(day) && day.Length == 2)
                        {
                            if (num.IsMatch(year) && year.Length == 4)
                            {
                                if (DateTime.TryParseExact(input, "mm/dd/yyyy", null, DateTimeStyles.None, out DateTime date))
                                {
                                    valid = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        static bool Null(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            else
                return true;
        }

        static void Valid(string ID, bool valid)
        {
            if (valid)
            {
                Console.WriteLine("{0} is valid!\n", ID);
            }
            else
                Console.WriteLine("Sorry, {0} is not valid!\n  ", ID);
        }

        static bool Retry()
        {
            Console.WriteLine("Enter new information? (y/n)  ");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.Y)
            {
                return true;
            }
            else if (key == ConsoleKey.N)
            {
                Console.WriteLine("Goodbye! Press ESCAPE to exit.");
                return false;
            }
            else
            {
                Console.WriteLine("Invalid Input, Please Try Again...");
                bool retry = Retry();
                return retry;
            }
        }

        static void Exit()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Exit();
            }
        }
    }
}//October 15-16, 2018
