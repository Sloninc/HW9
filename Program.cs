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
        static void Main()
        {
            try
            {
                ConsoleKeyInfo ki;
                bool ainput = true, binput = true, cinput = true;
                selectedValue = 1;
                double a = 0, b = 0, c = 0,preb=0;
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
                            Console.SetCursorPosition(0, selectedValue);
                            Console.WriteLine(new string(' ', 60));
                            Console.SetCursorPosition(0, selectedValue);
                            Console.Write($" {options[selectedValue - 1]}: {ki.KeyChar.ToString()}");
                            if (selectedValue == 1)
                            {
                                a = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine(new string(' ', 60));
                                Console.SetCursorPosition(0, 0);
                                if (Regex.IsMatch(equality, @"^\S+\*x\^2"))
                                {
                                    if (a == 0)
                                    {
                                        Regex regex1 = new Regex(@"^\S+\*x\^2\+?");
                                        equality = regex1.Replace(equality, "");
                                    }
                                    else
                                    {
                                        Regex regex = new Regex(@"^\S+\*x\^2");
                                        equality = regex.Replace(equality, $"{a}*x^2");
                                    }
                                }
                                else
                                {
                                    if (a == 0)
                                    {
                                        if (Regex.IsMatch(equality, @"^-?x\^2"))
                                        {
                                            Regex regex = new Regex(@"^-?x\^2\+?");
                                            equality = regex.Replace(equality, $"");
                                        }
                                    }
                                    else
                                    {
                                        if (Regex.IsMatch(equality, @"^-?x\^2"))
                                        {
                                            Regex regex = new Regex(@"^-?x\^2");
                                            equality = regex.Replace(equality, $"{a}*x^2");
                                        }
                                        else equality = Regex.IsMatch(equality, @"^-\w") ? equality.Insert(0, $"{a}*x^2") : equality.Insert(0, $"{a}*x^2+");
                                    }
                                    
                                }
                                if (a == 1 || a == -1)
                                    equality = (a == 1) ? equality.Replace("1*x^", "x^") : equality.Replace("-1*x^", "-x^");
                                Console.WriteLine(equality);
                                ainput = false;
                            }
                            else if(selectedValue == 2)
                            {
                                
                                b = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine(new string(' ', 60));
                                Console.SetCursorPosition(0, 0);
                                Regex regex = (preb==1||preb==-1)? new Regex(@"(\+|\-)?x(?(?=\^)2)") : new Regex(@"(\+|\-)?[0-9b\,]+\*x(?(?=\^)2)");
                                if (Regex.IsMatch(equality, @"x[^^]"))
                                {
                                    if (b == 0)
                                    {
                                        if (a == 0&& !ainput)
                                            equality = cinput ? "c=0":$"{c}=0";
                                        else
                                            equality = regex.Replace(equality, "");
                                    }
                                    else
                                    {
                                        if(a == 0 && !ainput)
                                        {
                                            equality = (b > 0) ? regex.Replace(equality, (b == 1) ? $"x" : $"{b}*x") : regex.Replace(equality, (b == -1) ? $"-x" : $"{b}*x");
                                        }
                                        else
                                        {
                                            equality = (b > 0) ? regex.Replace(equality, (b == 1) ? $"+x" : $"+{b}*x") : regex.Replace(equality, (b == -1) ? $"-x" : $"{b}*x");
                                        }
                                    }
                                }
                                else
                                {
                                    if (b == 0) { }
                                    else
                                    {
                                        if (a == 0 && !ainput)
                                            equality = (c > 0||cinput) ? equality.Insert(0, $"{b}*x+") : equality.Insert(0, $"{b}*x");

                                        else if(ainput)
                                            equality = (b > 0) ? equality.Insert("a*x^2".Length, $"+{b}*x") : equality.Insert("a*x^2".Length, $"{b}*x");
                                        else
                                            equality = equality.Insert($"{a}*x^2".Length, $"{b}*x");
                                    }

                                }
                                if (b == 1||b==-1)
                                {
                                    equality = (b==1) ? regex.Replace(equality, "+x") : regex.Replace(equality, "-x");
                                }
                                Console.WriteLine(equality);
                                preb = b;
                                binput = false;
                            }
                            else
                            {
                                c = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine(new string(' ', 60));
                                Console.SetCursorPosition(0, 0);
                                Regex regex = new Regex(@"(\+|\-)+(c?|[0-9]?\,?[0-9]+)=");
                                if (Regex.IsMatch(equality, @"(\+|\-)+(c?|[0-9]?\,?[0-9]+)="))
                                {
                                    if (c == 0) 
                                    {
                                            equality = regex.Replace(equality, "=");
                                    }
                                    else 
                                    {
                                        if (a == 0 && !ainput && b==0 && !binput)
                                        {
                                            equality = regex.Replace(equality, $"{c}=");
                                        }
                                        else
                                        {
                                            equality = (c > 0) ? regex.Replace(equality, $"+{c}=") : regex.Replace(equality, $"{c}=");
                                        }
                                    }
                                }
                                else
                                {
                                    if (c == 0) { }
                                    else
                                    {
                                        equality = (c > 0) ? equality.Insert(equality.IndexOf('='), $"+{c}") : equality.Insert(equality.IndexOf('='), $"{c}");
                                    }

                                }
                                Console.WriteLine(equality);
                                cinput = false;
                            }
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