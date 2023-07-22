using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace HW9
{
    internal class Program
    {
        /// <summary>
        /// Строка уравнения
        /// </summary>
        private static string _equality="a*x^2+b*x+c=0";
        /// <summary>
        /// Исходное положение стрелки меню
        /// </summary>
        private static int _selectedValue;
        /// <summary>
        /// Введённый пользователем символ
        /// </summary>
        private static ConsoleKeyInfo _ki;
        /// <summary>
        /// Коэффициенты уравнения
        /// </summary>
        private static int _a, _b, _c;
        /// <summary>
        /// Флаги определения коэффициентов
        /// </summary>
        private static bool _aInput, _bInput, _cInput;
        /// <summary>
        /// Строка данных, введённых пользователем.
        /// </summary>
        private static string _tempword="";

        /// <summary>
        /// Очистка экрана и инициализация полей
        /// </summary>
        #region Initialize
        static void Initialize()
        {
            Console.Clear();
            _equality = "a*x^2+b*x+c=0";
            _selectedValue = 0;
            _a = 0;
            _b = 0;
            _c = 0;
            _aInput = true;
            _bInput = true;
            _cInput = true;
            _tempword = "";
        }
        #endregion

        #region Main
        static void Main()
        {
            Initialize();
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
                int[] abc = { _a, _b, _c };
                bool[] abcinput = { _aInput, _bInput, _cInput };
                for(int i = 0; i < options.Length; i++)
                {
                    if (i == _selectedValue - 1)
                    {
                        fe.Data[options[i]] = _tempword;
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
                oe.Data[options[_selectedValue - 1]] = _tempword;
                string message = $"введенное значение \"{options[_selectedValue - 1]}\" не вмещается в тип int, значение должно быть в дипазоне от -2 147 483 648 до 2 147 483 647";
                FormatData(message, Severity.Note, oe.Data);
            }
            Console.WriteLine("Для продолжения нажмите \"Enter\", для выхода нажмите \"Escape\"");
            while (true)
            {
                ConsoleKeyInfo key= Console.ReadKey();  
                if (key.Key == ConsoleKey.Enter)
                    Main();
                if (key.Key == ConsoleKey.Escape)
                    return;
                ClearString(Console.CursorTop);
            }
        }
        #endregion

        /// <summary>
        /// Выбор пользователем коэффициента для ввода значения
        /// </summary>
        #region Select
        private static void Select()
        {
            _selectedValue = 1;
            PrintMenu();
            WriteCursor(_selectedValue);
            do
            {
                _ki = Console.ReadKey();
                if(_ki.Key == ConsoleKey.Escape)
                    Escape();
                ClearCursor(_selectedValue);
                switch (_ki.Key)
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
                WriteCursor(_selectedValue);
            } while (_aInput || _bInput || _cInput);
        }
        #endregion

        /// <summary>
        /// Вывод результата - значения корней уравнения.
        /// </summary>
        #region Result
        private static void Result()
        {
            Console.SetCursorPosition(0, 4);
            QuadraticEquation func = new QuadraticEquation(_a, _b, _c);
            var result = func.Roots;
            if (result.Count == 2)
                Console.WriteLine($"Корни уравнения {_equality} равны: x1={result[0]} x2={result[1]}");
            if (result.Count == 1)
                Console.WriteLine($"Корень уравнения {_equality} равен: x={result[0]}");
        }
        #endregion

        /// <summary>
        /// Построение входных данных
        /// </summary>
        #region BuildInput
        private static void BuildInput()
        {
            ClearString(_selectedValue);
            Console.Write($" {options[_selectedValue - 1]}: {_ki.KeyChar.ToString()}");
            StringBuilder wordbuild = new StringBuilder(_ki.KeyChar.ToString());
            ConsoleKeyInfo letter;
            int cursor = Console.CursorLeft;
            do
            {
                letter = Console.ReadKey();
                switch (letter.Key)
                {
                    case ConsoleKey.Escape:
                        Escape();
                        break;
                    case ConsoleKey.Backspace:
                        wordbuild = cursor > 4 ? wordbuild.Remove(cursor - 5, 1) : wordbuild;
                        cursor = cursor > 4 ? cursor - 1 : cursor;
                        ClearString(_selectedValue);
                        Console.Write($" {options[_selectedValue - 1]}: {wordbuild.ToString()}");
                        Console.SetCursorPosition(cursor, _selectedValue);
                        break;
                    case ConsoleKey.LeftArrow:
                        cursor = Console.CursorLeft > 4 ? Console.CursorLeft - 1 : Console.CursorLeft;
                        Console.SetCursorPosition(cursor, _selectedValue);
                        break;
                    case ConsoleKey.RightArrow:
                        cursor = Console.CursorLeft < 4 + wordbuild.Length ? Console.CursorLeft + 1 : Console.CursorLeft;
                        Console.SetCursorPosition(cursor, _selectedValue);
                        break;
                    default:
                        if (Regex.IsMatch(letter.KeyChar.ToString(), @"\S+"))
                        {
                            if (cursor < 4 + wordbuild.Length)
                            {
                                wordbuild.Replace(wordbuild[cursor - 4], letter.KeyChar, cursor - 4, 1);
                                cursor++;
                                ClearString(_selectedValue);
                                Console.Write($" {options[_selectedValue - 1]}: {wordbuild.ToString()}");
                                Console.SetCursorPosition(cursor, _selectedValue);
                            }
                            else
                            {
                                wordbuild.Append(letter.KeyChar);
                                ClearString(_selectedValue);
                                Console.Write($" {options[_selectedValue - 1]}: {wordbuild.ToString()}");
                                cursor = Console.CursorLeft;
                            }
                        }
                        else
                        {
                            ClearString(_selectedValue);
                            Console.Write($" {options[_selectedValue - 1]}: {wordbuild.ToString()}");
                            cursor = Console.CursorLeft;
                        }
                        break;
                }
            }
            while (letter.Key != ConsoleKey.Enter);
            _tempword = wordbuild.ToString();
        }
        #endregion

        /// <summary>
        /// Опция завершения программы при нажатии Escape
        /// </summary>
        #region Escape
        static void Escape()
        {
            Console.Clear();
            Console.Write("\x1B[1D"); // убрать символ
            Console.Write("\x1B[1P"); // Escape
            Console.WriteLine("Завершить работу программы? y/n");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.N)
                    Main();
                if (key.Key == ConsoleKey.Y)
                   Environment.Exit(0);
                ClearString(Console.CursorTop);
            }
        }
        #endregion

        /// <summary>
        /// Формирование и вывод квадратного уравнения с пользовательскими данными
        /// </summary>
        /// <exception cref="FormatException"></exception>
        #region PrintEquality
        private static void PrintEquality()
        {
            switch (_selectedValue)
            {
                case 1:
                    _a=int.Parse(_tempword);
                    ClearString(0);
                    string aword = $"{_a}*x^2";
                    Regex areg = new Regex(@"^-?(a?|[0-9]*)\*?x\^2");
                    if (Regex.IsMatch(_equality, @"^-?(a?|[0-9]*)\*?x\^"))
                    {
                        _equality = (_a == 0) ? areg.Replace(_equality, "") : areg.Replace(_equality, aword);
                        if (_equality[0] == '+')
                            _equality = _equality.Substring(1);
                    }
                    else
                    {
                        if (_a == 0) { }
                        else _equality = Regex.IsMatch(_equality, @"^-\w") ? _equality.Insert(0, aword) : _equality.Insert(0, aword + "+");
                    }
                    if (_a == 1 || _a == -1)
                        _equality = (_a == 1) ? _equality.Replace("1*x^", "x^") : _equality.Replace("-1*x^", "-x^");
                    Console.WriteLine(_equality);
                    _aInput = false;
                    break;
                case 2:
                    _b = int.Parse(_tempword);
                    ClearString(0);
                    string bword = (_b > 0) ? $"+{_b}*x" : $"{_b}*x";
                    Regex breg = new Regex(@"(\+|\-)?(b?|[0-9]*)\*?x(?!\^)");
                    if (Regex.IsMatch(_equality, @"x[^^]"))
                    {
                        _equality = (_b == 0) ? breg.Replace(_equality, "") : breg.Replace(_equality, bword);
                        if (_equality[0] == '+')
                            _equality = _equality.Substring(1);
                    }
                    else
                    {
                        if (_b == 0) { }
                        else
                        {
                            string bcword = _c > 0 || _cInput ? _equality.Insert(0, $"{_b}*x+") : _equality.Insert(0, $"{_b}*x");
                            _equality = (_a == 0 && !_aInput) ? bcword : _aInput ? _equality.Insert("a*x^2".Length, bword) : _equality.Insert($"{_a}*x^2".Length, bword);
                        }
                    }
                    if (_b == 1 || _b == -1)
                        _equality = (_b == 1) ? _equality.Replace("1*x", "x") : _equality.Replace("-1*x", "-x");
                    Console.WriteLine(_equality);
                    _bInput = false;
                    break;
                case 3:
                    _c = int.Parse(_tempword);
                    ClearString(0);
                    Regex creg = new Regex(@"(\+|\-)+(c?|[0-9]*)=");
                    if (Regex.IsMatch(_equality, @"(\+|\-)+(c?|[0-9]*)="))
                    {
                        string cword = _c > 0 ? creg.Replace(_equality, $"+{_c}=") : creg.Replace(_equality, $"{_c}=");
                        if (_a == 0 && !_aInput && _b == 0 && !_bInput)
                            _equality = creg.Replace(_equality, $"{_c}=");
                        else _equality = _c == 0 ? creg.Replace(_equality, "=") : cword;
                    }
                    else
                    {
                        if (_c == 0) { }
                        else
                            _equality = (_c > 0) ? _equality.Insert(_equality.IndexOf('='), $"+{_c}") : _equality.Insert(_equality.IndexOf('='), $"{_c}");
                    }
                    Console.WriteLine(_equality);
                    _cInput = false;
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Тип исключения
        /// </summary>
        #region Severity
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
            if (_selectedValue < options.Length)
            {
                _selectedValue++;
            }
            else
            {
                _selectedValue = 1;
            }
        }
        #endregion

        /// <summary>
        /// На одну строку вверх
        /// </summary>
        #region SetUp
        private static void SetUp()
        {
            if (_selectedValue > 1)
            {
                _selectedValue--;
            }
            else
            {
                _selectedValue = 3;
            }
        }
        #endregion

        /// <summary>
        /// Вывести меню квадратного уравнения
        /// </summary>
        #region PrintMenu
        private static void PrintMenu()
        {
            Console.WriteLine(_equality);
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