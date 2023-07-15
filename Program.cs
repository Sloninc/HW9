using System.Text.RegularExpressions;
namespace HW9
{
    internal class Program
    {

       static string equality = "a*x^2+b*x+c=0";
        /// <summary>
        /// Исходное положение стрелки меню
        /// </summary>
        private static int selectedValue = 0;
        private static ConsoleKeyInfo ki;
        private static bool ainput = true, binput = true, cinput = true;
        private static double a = 0, b = 0, c = 0;
        static void Main()
        {
            try
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
                            PrintEquality();
                            break;
                    }
                    WriteCursor(selectedValue);
                } while (ainput||binput||cinput);
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
                Console.WriteLine(de.Message);
            }
            catch(FormatException fe)
            {
                Console.WriteLine(fe.Message);
            }
            Console.ReadLine();
        }

        private static void PrintEquality()
        {
            ClearString(selectedValue);
            Console.Write($" {options[selectedValue - 1]}: {ki.KeyChar.ToString()}");
            switch (selectedValue)
            {
                case 1:
                    a = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                    ClearString(0);
                    string aword = $"{a}*x^2";
                    Regex areg = new Regex(@"^\S*\*?x\^2");
                    if (Regex.IsMatch(equality, @"^\S*\*?x\^2"))
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
                    b = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                    ClearString(0);
                    string bword = (b > 0) ? $"+{b}*x" : $"{b}*x";
                    Regex breg = new Regex(@"(\+|\-)?(b?|[0-9]*[.,]?[0-9]*)\*?x(?!\^)");
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
                    c = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                    ClearString(0);
                    Regex creg = new Regex(@"(\+|\-)+(c?|[0-9]*[.,]?[0-9]+)=");
                    if (Regex.IsMatch(equality, @"(\+|\-)+(c?|[0-9]*[.,]?[0-9]+)="))
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