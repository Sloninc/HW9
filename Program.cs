namespace HW9
{
    internal class Program
    {
        /// <summary>
        /// Исходное положение стрелки меню
        /// </summary>
        private static int selectedValue = 0;
        static void Main()
        {
            try
            {
                string s = "a*x^2+b*x+c=0";
                ConsoleKeyInfo ki;
                bool ainput = true, binput = true, cinput = true;
                selectedValue = 1;
                double a = 0, b = 0, c = 0;
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
                            //Console.Write(ki.KeyChar.ToString());
                            if (selectedValue == 1)
                            {
                                a = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine(new string(' ', 60));
                                Console.SetCursorPosition(0, 0);
                                s = s.Remove(0, s.IndexOf('*'));
                                s =s.Insert(0,a.ToString());
                                Console.WriteLine(s);
                                ainput = false;
                            }
                            else if(selectedValue == 2)
                            {
                                b = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine(new string(' ', 60));
                                Console.SetCursorPosition(0, 0);
                                s = s.Remove(s.IndexOf('+')+1, s.LastIndexOf('*')-1-s.IndexOf('+'));
                                s = s.Insert(s.IndexOf('+')+1, b.ToString());
                                Console.WriteLine(s);
                                binput = false;
                            }
                            else
                            {
                                c = double.Parse(ki.KeyChar.ToString() + Console.ReadLine());
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine(new string(' ', 60));
                                Console.SetCursorPosition(0, 0);
                                s = s.Remove(s.LastIndexOf('+')+1, s.IndexOf('=')-1-s.LastIndexOf('+'));
                                s = s.Insert(s.LastIndexOf('+')+1, c.ToString());
                                Console.WriteLine(s);
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
                    Console.WriteLine($"Корни уравнения {s} равны: x1={result[0]} x2={result[1]}");
                if(result.Count == 1)
                    Console.WriteLine($"Корень уравнения {s} равен: x={result[0]}");
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
            Console.WriteLine("a*x^2+b*x+c=0");
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