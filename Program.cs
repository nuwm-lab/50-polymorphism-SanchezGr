using System;

using System;

using System.Globalization;

namespace LabWork
{
    class Praktykant
    {
        protected string prizv;
        protected string imya;
        protected string vuz;

        protected static string ReadLetters(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = (Console.ReadLine() ?? "").Trim();
                if (s.Length == 0) { Console.WriteLine("Введіть дані."); continue; }
                bool ok = true;
                foreach (char c in s)
                    if (!(char.IsLetter(c) || c == ' ' || c == '-' || c == '\'' || c == '’')) { ok = false; break; }
                if (ok) return s;
                Console.WriteLine("Помилка: дозволені лише літери, пробіл, дефіс, апостроф.");
            }
        }

        protected bool IsSym(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            string t = "";
            foreach (char c in s) if (char.IsLetter(c)) t += char.ToLowerInvariant(c);
            if (t.Length == 0) return false;
            int l = 0, r = t.Length - 1;
            while (l < r) { if (t[l] != t[r]) return false; l++; r--; }
            return true;
        }

        public virtual void InputMain()
        {
            prizv = ReadLetters("Прізвище практиканта: ");
            imya  = ReadLetters("Ім'я практиканта: ");
            vuz   = ReadLetters("ВНЗ: ");
        }

        public virtual void Show()
        {
            Console.WriteLine($"\nПрактикант: {imya} {prizv}");
            Console.WriteLine($"ВНЗ: {vuz}");
            Console.WriteLine($"Симетричне прізвище: {(IsSym(prizv) ? "Так" : "Ні")}");
        }
    }

    class PracivnykFirmy : Praktykant
    {
        private string zaklad;
        private string posada;
        private DateTime dataPrijomu;

        static DateTime ReadDate(string prompt)
        {
            string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy" };
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    return dt.Date;
                Console.WriteLine("Неправильний формат дати. Приклади: 2023-09-01 або 01.09.2023");
            }
        }

        void Staj(out int y, out int m, out int d)
        {
            var now = DateTime.Today;
            if (now < dataPrijomu) { y = m = d = 0; return; }
            y = now.Year - dataPrijomu.Year;
            m = now.Month - dataPrijomu.Month;
            d = now.Day - dataPrijomu.Day;
            if (d < 0) { var prev = now.AddMonths(-1); d += DateTime.DaysInMonth(prev.Year, prev.Month); m--; }
            if (m < 0) { m += 12; y--; }
        }

        public override void InputMain()
        {
            // базові поля читаємо з іншими підказками
            prizv = ReadLetters("Прізвище працівника: ");
            imya  = ReadLetters("Ім'я працівника: ");
            vuz   = ReadLetters("ВНЗ: ");

            zaklad = ReadLetters("Заклад, який закінчив: ");
            posada = ReadLetters("Посада: ");
            dataPrijomu = ReadDate("Дата прийому (yyyy-MM-dd або dd.MM.yyyy): ");
        }

        public override void Show()
        {
            Console.WriteLine($"\nПрацівник фірми: {imya} {prizv}");
            Console.WriteLine($"Посада: {posada}");
            Console.WriteLine($"ВНЗ: {vuz}");
            Console.WriteLine($"Закінчив: {zaklad}");
            Console.WriteLine($"Дата прийому: {dataPrijomu:yyyy-MM-dd}");
            Staj(out int y, out int m, out int d);
            Console.WriteLine($"Стаж роботи: {y} р. {m} міс. {d} дн.");
            Console.WriteLine($"Симетричне прізвище: {(IsSym(prizv) ? "Так" : "Ні")}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Оберіть режим (1 — працівник, 2 — практикант): ");
            string choose = Console.ReadLine();

            Praktykant obj;
            if (choose == "1") obj = new PracivnykFirmy();
            else if (choose == "2") obj = new Praktykant();
            else { Console.WriteLine("Невірний вибір."); return; }

            obj.InputMain();
            obj.Show();
        }
    }
}
