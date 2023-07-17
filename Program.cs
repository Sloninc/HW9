using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace HW9
{
    internal class Program
    {

        private static string equality = "a*x^2+b*x+c=0";
        /// <summary>
        /// Исходное положение стрелки меню
        /// </summary>
        private static int selectedValue = 0;
        private static ConsoleKeyInfo ki;
        private static int a = 0, b = 0, c = 0;
        private static bool ainput=true,binput=true,cinput=true;  
        static void Main()
        {
            try
            {
                selectedValue = 1;
                PrintMenu();
                WriteCursor(selectedValue);
                //bool isNoneAllInput = true;
                do
                {

                    ki = Console.ReadKey();
                    ClearCursor(selectedValue);
                    switch (ki.Key)
                    {
                        case ConsoleKey.UpArrow:
                            SetUp();
                            break;
                        case ConsoleKey.DownArrow:
                            SetDown();
                            break;
                        default:
                            PrintEquality();
                            break;
                    }
                    WriteCursor(selectedValue);
                } while (ainput || binput || cinput);
                Console.SetCursorPosition(0, 4);
                QuadraticEquation func = new QuadraticEquation(a, b, c);
                var result = func.Roots;
                if (result.Count == 2)
                    Console.WriteLine($"Корни уравнения {equality} равны: x1={result[0]} x2={result[1]}");
                if(result.Count == 1)
                    Console.WriteLine($"Корень уравнения {equality} равен: x={result[0]}");
            }
            catch(DiscriminantException de)
            {
                FormatData(de.Message, Severity.Warning, de.Data);
            }
            catch(FormatException fe)
            {
                Console.WriteLine(fe.Message);
            }
            catch(OverflowException oe)
            {
                Console.WriteLine(oe.Message);
            }
            Console.ReadLine();
        }

        private static void PrintEquality()
        {
            ClearString(selectedValue);
            Console.Write($" {options[selectedValue - 1]}: {ki.KeyChar.ToString()}");
            StringBuilder s = new StringBuilder(ki.KeyChar.ToString());
            ConsoleKeyInfo cc;
            int cursor = Console.CursorLeft;
            do
            {
                cc = Console.ReadKey();
                if (cc.Key == ConsoleKey.Backspace)
                {
                    s = (cursor > 4) ? s.Remove(cursor-5, 1) : s;
                    cursor = (cursor > 4) ? cursor - 1 : cursor;
                    ClearString(selectedValue);
                    Console.Write($" {options[selectedValue - 1]}: {s.ToString()}");
                    Console.SetCursorPosition(cursor, selectedValue);
                }
                else if (cc.Key == ConsoleKey.LeftArrow)
                {
                    cursor = (Console.CursorLeft > 4) ? Console.CursorLeft - 1 : Console.CursorLeft; 
                    Console.SetCursorPosition(cursor, selectedValue);
                }
                else if (cc.Key == ConsoleKey.RightArrow)
                {
                    cursor = (Console.CursorLeft < 4+s.Length) ? Console.CursorLeft + 1 : Console.CursorLeft;
                    Console.SetCursorPosition(cursor, selectedValue);
                }
                else if(Regex.IsMatch(cc.KeyChar.ToString(), @"\S+"))
                {
                    if (cursor < 4 + s.Length)
                    {
                        s.Replace(s[cursor - 4], cc.KeyChar, cursor - 4, 1);
                        cursor++;
                        ClearString(selectedValue);
                        Console.Write($" {options[selectedValue - 1]}: {s.ToString()}");
                        Console.SetCursorPosition(cursor, selectedValue);
                    }
                    else
                    {
                        s.Append(cc.KeyChar);
                        ClearString(selectedValue);
                        Console.Write($" {options[selectedValue - 1]}: {s.ToString()}");
                        cursor = Console.CursorLeft;
                    }
                }
                else
                {
                    ClearString(selectedValue);
                    Console.Write($" {options[selectedValue - 1]}: {s.ToString()}");
                    cursor = Console.CursorLeft;
                }
            }
            while (cc.Key != ConsoleKey.Enter);
            string ss=s.ToString();
            switch (selectedValue)
            {
                case 1:
                    //a = int.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                    a = int.Parse(ss);
                    ClearString(0);
                    string aword = $"{a}*x^2";
                    Regex areg = new Regex(@"^-?(a?|[0-9]*)\*?x\^2");
                    if (Regex.IsMatch(equality, @"^-?(a?|[0-9]*)\*?x\^"))
                    {
                        equality = (a == 0) ? areg.Replace(equality, "") : areg.Replace(equality, aword);
                        if (equality[0] == '+')
                            equality = equality.Substring(1);
                    }
                    else
                    {
                        if (a == 0) { }
                        else equality = Regex.IsMatch(equality, @"^-\w") ? equality.Insert(0, aword) : equality.Insert(0, aword + "+");
                    }
                    if (a == 1 || a == -1)
                        equality = (a == 1) ? equality.Replace("1*x^", "x^") : equality.Replace("-1*x^", "-x^");
                    Console.WriteLine(equality);
                    ainput = false;
                    break;
                case 2:
                    //b = int.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                    b = int.Parse(ss);
                    ClearString(0);
                    string bword = (b > 0) ? $"+{b}*x" : $"{b}*x";
                    Regex breg = new Regex(@"(\+|\-)?(b?|[0-9]*)\*?x(?!\^)");
                    if (Regex.IsMatch(equality, @"x[^^]"))
                    {
                        equality = (b == 0) ? breg.Replace(equality, "") : breg.Replace(equality, bword);
                        if (equality[0] == '+')
                            equality = equality.Substring(1);
                    }
                    else
                    {
                        if (b == 0) { }
                        else
                        {
                            string bcword = c > 0 || cinput ? equality.Insert(0, $"{b}*x+") : equality.Insert(0, $"{b}*x");
                            equality = (a == 0 && !ainput) ? bcword : ainput ? equality.Insert("a*x^2".Length, bword) : equality.Insert($"{a}*x^2".Length, bword);
                        }
                    }
                    if (b == 1 || b == -1)
                        equality = (b == 1) ? equality.Replace("1*x", "x") : equality.Replace("-1*x", "-x");
                    Console.WriteLine(equality);
                    binput = false;
                    break;
                case 3:
                    //c = int.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                    c = int.Parse(ss);
                    ClearString(0);
                    Regex creg = new Regex(@"(\+|\-)+(c?|[0-9]*)=");
                    if (Regex.IsMatch(equality, @"(\+|\-)+(c?|[0-9]*)="))
                    {
                        string cword = c > 0 ? creg.Replace(equality, $"+{c}=") : creg.Replace(equality, $"{c}=");
                        if (a == 0 && !ainput && b == 0 && !binput)
                            equality = creg.Replace(equality, $"{c}=");
                        else equality = c == 0 ? creg.Replace(equality, "=") : cword;
                    }
                    else
                    {
                        if (c == 0) { }
                        else
                            equality = (c > 0) ? equality.Insert(equality.IndexOf('='), $"+{c}") : equality.Insert(equality.IndexOf('='), $"{c}");
                    }
                    Console.WriteLine(equality);
                    cinput = false;
                    break;
            }
        }

        public enum Severity
        {
            Warning,
            Error
        }

        public static void FormatData(string message, Severity severity, IDictionary<string,string> data)
        {
            if (severity.Equals(Severity.Warning))
            {
                Console.SetCursorPosition(0, 5);
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(new string('-', 50));
                Console.WriteLine(message);
                Console.WriteLine(new string('-', 50));
                Console.WriteLine();
                foreach(var item in data)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("хотите продолжить? нажмите y-да/n-нет");
            ConsoleKeyInfo yn;
            do
            {
                yn = Console.ReadKey();
            }
            while (yn.Key!=ConsoleKey.Y && yn.Key!=ConsoleKey.N);
            if (yn.Key == ConsoleKey.Y)
            {
                Console.Clear();
                Main();
            }
            else return;
        }
        private static void ClearString(int numstring)
        {
            Console.SetCursorPosition(0, numstring);
            Console.WriteLine(new string(' ', 60));
            Console.SetCursorPosition(0, numstring);
        }
        /// <summary>
        /// Опции меню
        /// </summary>
        private static string[] options = new[]{
            "a",
            "b",
            "c"
        };

        /// <summary>
        /// На одну строку вниз
        /// </summary>
        private static void SetDown()
        {
            if (selectedValue < options.Length)
            {
                selectedValue++;
            }
            else
            {
                selectedValue = 1;
            }
        }

        /// <summary>
        /// На одну строку вверх
        /// </summary>
        private static void SetUp()
        {
            if (selectedValue > 1)
            {
                selectedValue--;
            }
            else
            {
                selectedValue = 3;
            }
        }

        /// <summary>
        /// Вывести меню квадратного уравнения
        /// </summary>
        private static void PrintMenu()
        {
            Console.WriteLine(equality);
            for (var i = 0; i < options.Length; i++)
            {
                Console.WriteLine($" {options[i]}:");
            }
        }



   

        private static void WriteCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(">");
            Console.SetCursorPosition(0, pos);
        }

        private static void ClearCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(" ");
            Console.SetCursorPosition(0, pos);
        }
    }
}