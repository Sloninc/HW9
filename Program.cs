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
        private static string tempword = "";
        static void Main()
        {
            try
            {
                Select();
                Result();  
            }
            catch(DiscriminantException de)
            {
                FormatData(de.Message, Severity.Warning, de.Data);
            }
            catch(FormatException fe)
            {
                string message = "";
                int[] abc = { a, b, c };
                bool[] abcinput = { ainput, binput, cinput };
                for(int i = 0; i < options.Length; i++)
                {
                    if (i == selectedValue - 1)
                    {
                        fe.Data[(options[i])] = tempword;
                        message = $"Неверный формат параметра {options[i]}";
                    }
                    else
                    {
                        fe.Data[options[i]] = !abcinput[i] ? abc[i].ToString() : "не определён";    
                    }
                }
                FormatData(message, Severity.Error, fe.Data);
            }
            catch(OverflowException oe)
            {
                string message = "";
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedValue - 1)
                    {
                        oe.Data[(options[i])] = tempword;
                        message = $"введенное значение \"{options[i]}\" не вмещается в тип int, значение должно быть в дипазоне от -2 147 483 648 до 2 147 483 647";
                    }
                }
                FormatData(message, Severity.Note, oe.Data);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Выбор пользователем коэффициента для ввода значения
        /// </summary>
        #region Select
        private static void Select()
        {
            selectedValue = 1;
            PrintMenu();
            WriteCursor(selectedValue);
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
                        BuildInput();
                        PrintEquality();
                        break;
                }
                WriteCursor(selectedValue);
            } while (ainput || binput || cinput);
        }
        #endregion

        /// <summary>
        /// Вывод результата - значения корней уравнения.
        /// </summary>
        #region Result
        private static void Result()
        {
            Console.SetCursorPosition(0, 4);
            QuadraticEquation func = new QuadraticEquation(a, b, c);
            var result = func.Roots;
            if (result.Count == 2)
                Console.WriteLine($"Корни уравнения {equality} равны: x1={result[0]} x2={result[1]}");
            if (result.Count == 1)
                Console.WriteLine($"Корень уравнения {equality} равен: x={result[0]}");
        }
        #endregion

        /// <summary>
        /// Построение входных данных
        /// </summary>
        #region BuildInput
        private static void BuildInput()
        {
            ClearString(selectedValue);
            Console.Write($" {options[selectedValue - 1]}: {ki.KeyChar.ToString()}");
            StringBuilder wordbuild = new StringBuilder(ki.KeyChar.ToString());
            ConsoleKeyInfo letter;
            int cursor = Console.CursorLeft;
            do
            {
                letter = Console.ReadKey();
                switch (letter.Key)
                {
                    case ConsoleKey.Backspace:
                        wordbuild = (cursor > 4) ? wordbuild.Remove(cursor - 5, 1) : wordbuild;
                        cursor = (cursor > 4) ? cursor - 1 : cursor;
                        ClearString(selectedValue);
                        Console.Write($" {options[selectedValue - 1]}: {wordbuild.ToString()}");
                        Console.SetCursorPosition(cursor, selectedValue);
                        break;
                    case ConsoleKey.LeftArrow:
                        cursor = (Console.CursorLeft > 4) ? Console.CursorLeft - 1 : Console.CursorLeft;
                        Console.SetCursorPosition(cursor, selectedValue);
                        break;
                    case ConsoleKey.RightArrow:
                        cursor = (Console.CursorLeft < 4 + wordbuild.Length) ? Console.CursorLeft + 1 : Console.CursorLeft;
                        Console.SetCursorPosition(cursor, selectedValue);
                        break;
                    default:
                        if (Regex.IsMatch(letter.KeyChar.ToString(), @"\S+"))
                        {
                            if (cursor < 4 + wordbuild.Length)
                            {
                                wordbuild.Replace(wordbuild[cursor - 4], letter.KeyChar, cursor - 4, 1);
                                cursor++;
                                ClearString(selectedValue);
                                Console.Write($" {options[selectedValue - 1]}: {wordbuild.ToString()}");
                                Console.SetCursorPosition(cursor, selectedValue);
                            }
                            else
                            {
                                wordbuild.Append(letter.KeyChar);
                                ClearString(selectedValue);
                                Console.Write($" {options[selectedValue - 1]}: {wordbuild.ToString()}");
                                cursor = Console.CursorLeft;
                            }
                        }
                        else
                        {
                            ClearString(selectedValue);
                            Console.Write($" {options[selectedValue - 1]}: {wordbuild.ToString()}");
                            cursor = Console.CursorLeft;
                        }
                        break;
                }
            }
            while (letter.Key != ConsoleKey.Enter);
            tempword = wordbuild.ToString();
        }
        #endregion

        /// <summary>
        /// Формирование и вывод квадратного уравнения с пользовательскими данными
        /// </summary>
        /// <exception cref="FormatException"></exception>
        #region PrintEquality
        private static void PrintEquality()
        {
            switch (selectedValue)
            {
                case 1:
                    var isParsed = int.TryParse(tempword,out int acoef);
                    if (isParsed) a = acoef;
                    else throw new FormatException(tempword);
                    
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
                    b = int.Parse(tempword);
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
                    c = int.Parse(tempword);
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
        #endregion

        /// <summary>
        /// Тип исключения
        /// </summary>
        #region 
        public enum Severity
        {
            Note,
            Warning,
            Error
        }
        #endregion

        /// <summary>
        /// Вывод информации об обработанных исключениях
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <param name="data"></param>
        #region FormatData
        public static void FormatData(string message, Severity severity, IDictionary data)
        {
                Console.SetCursorPosition(0, 5);
            switch (severity)
            {
                case Severity.Note:
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case Severity.Warning:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case Severity.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
            }
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(new string('-', 50));
                Console.WriteLine(message);
                Console.WriteLine(new string('-', 50));
                Console.WriteLine();
                foreach (var item in data.Keys)
                {
                    Console.WriteLine($"{item}: {data[item]}");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion

        /// <summary>
        /// Очистка строки
        /// </summary>
        /// <param name="numstring"></param>
        #region ClearString
        private static void ClearString(int numstring)
        {
            Console.SetCursorPosition(0, numstring);
            Console.WriteLine(new string(' ', 60));
            Console.SetCursorPosition(0, numstring);
        }
        #endregion

        /// <summary>
        /// Опции меню
        /// </summary>
        #region Options
        private static string[] options = new[]{
            "a",
            "b",
            "c"
        };
        #endregion

        /// <summary>
        /// На одну строку вниз
        /// </summary>
        #region SetDown
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
        #endregion

        /// <summary>
        /// На одну строку вверх
        /// </summary>
        #region SetUp
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
        #endregion

        /// <summary>
        /// Вывести меню квадратного уравнения
        /// </summary>
        #region PrintMenu
        private static void PrintMenu()
        {
            Console.WriteLine(equality);
            for (var i = 0; i < options.Length; i++)
            {
                Console.WriteLine($" {options[i]}:");
            }
        }
        #endregion


        #region WriteCursor
        private static void WriteCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(">");
            Console.SetCursorPosition(0, pos);
        }
        #endregion

        #region ClearCursor
        private static void ClearCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(" ");
            Console.SetCursorPosition(0, pos);
        }
        #endregion
    }
}