using System;
using System.Globalization;
using System.Text;

namespace LabWork
{
    public class Praktykant
    {
        public string LastName  { get; protected set; }
        public string FirstName { get; protected set; }
        public string University { get; protected set; }

        protected static string ReadLetters(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(s))
                {
                    Console.WriteLine("Введіть дані.");
                    continue;
                }

                bool ok = true;
                foreach (char c in s)
                {
                    if (!(char.IsLetter(c) || c == ' ' || c == '-' || c == '\'' || c == '’'))
                    {
                        ok = false; break;
                    }
                }
                if (ok) return s;
                Console.WriteLine("Помилка: дозволені лише літери, пробіл, дефіс, апостроф.");
            }
        }

        protected static bool IsSym(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;

            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
                if (char.IsLetter(c)) sb.Append(char.ToLowerInvariant(c));

            if (sb.Length == 0) return false;

            int l = 0, r = sb.Length - 1;
            while (l < r)
            {
                if (sb[l] != sb[r]) return false;
                l++; r--;
            }
            return true;
        }

        public virtual void InputMain()
        {
            LastName   = ReadLetters("Прізвище практиканта: ");
            FirstName  = ReadLetters("Ім'я практиканта: ");
            University = ReadLetters("ВНЗ: ");
        }

        public virtual void Show()
        {
            Console.WriteLine($"\nПрактикант: {FirstName} {LastName}");
            Console.WriteLine($"ВНЗ: {University}");
            Console.WriteLine($"Симетричне прізвище: {(IsSym(LastName) ? "Так" : "Ні")}");
        }
    }

    public class PracivnykFirmy : Praktykant
    {
        private string _graduatedSchool;
        private string _position;
        private DateTime _hireDate;

        private static DateTime ReadDate(string prompt)
        {
            string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy" };
            while (true)
            {
                Console.Write(prompt);
                string s = (Console.ReadLine() ?? "").Trim();
                if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out var dt))
                    return dt.Date;

                Console.WriteLine("Неправильний формат дати. Приклади: 2023-09-01 або 01.09.2023");
            }
        }

        private void Experience(out int y, out int m, out int d)
        {
            var now = DateTime.Today;
            if (now < _hireDate) { y = m = d = 0; return; }

            y = now.Year - _hireDate.Year;
            m = now.Month - _hireDate.Month;
            d = now.Day - _hireDate.Day;

            if (d < 0)
            {
                var prev = now.AddMonths(-1);
                d += DateTime.DaysInMonth(prev.Year, prev.Month);
                m--;
            }
            if (m < 0)
            {
                m += 12;
                y--;
            }
        }

        public override void InputMain()
        {
            LastName   = ReadLetters("Прізвище працівника: ");
            FirstName  = ReadLetters("Ім'я працівника: ");
            University = ReadLetters("ВНЗ: ");
            _graduatedSchool = ReadLetters("Заклад, який закінчив: ");
            _position       = ReadLetters("Посада: ");
            _hireDate       = ReadDate("Дата прийому (yyyy-MM-dd або dd.MM.yyyy): ");
        }

        public override void Show()
        {
            Console.WriteLine($"\nПрацівник фірми: {FirstName} {LastName}");
            Console.WriteLine($"Посада: {_position}");
            Console.WriteLine($"ВНЗ: {University}");
            Console.WriteLine($"Закінчив: {_graduatedSchool}");
            Console.WriteLine($"Дата прийому: {_hireDate:yyyy-MM-dd}");
            Experience(out int y, out int m, out int d);
            Console.WriteLine($"Стаж роботи: {y} р. {m} міс. {d} дн.");
            Console.WriteLine($"Симетричне прізвище: {(IsSym(LastName) ? "Так" : "Ні")}");
        }
    }

    public class Program
    {
        public static void Main()
        {
            Console.Write("Оберіть режим (1 — працівник, 2 — практикант): ");
            string choose = (Console.ReadLine() ?? "").Trim();

            Praktykant obj;
            if (choose == "1") obj = new PracivnykFirmy();
            else if (choose == "2") obj = new Praktykant();
            else { Console.WriteLine("Невірний вибір."); return; }

            obj.InputMain();
            obj.Show();
        }
    }
}

    }
}
