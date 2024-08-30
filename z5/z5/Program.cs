using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Text;

class Program
{
    static void Main()
    {

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        List<string> employees = new List<string> { "Иван Иванов", "Петр Петров", "Сидор Сидоров" };

        Dictionary<string, List<DateTime>> vacationDictionary = new Dictionary<string, List<DateTime>>();

        foreach (var employee in employees)
        {
            vacationDictionary[employee] = new List<DateTime>();
        }

        List<DayOfWeek> workingDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

        Random random = new Random();
        DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
        DateTime endOfYear = new DateTime(DateTime.Now.Year, 12, 31);

        foreach (var employee in employees)
        {
            int remainingDays = 28;

            while (remainingDays > 0)
            {
                int vacationLength = remainingDays >= 14 ? (random.Next(0, 2) == 0 ? 7 : 14) : 7;

               DateTime startDate;
                do
                {
                    startDate = startOfYear.AddDays(random.Next((endOfYear - startOfYear).Days));
                } while (!workingDays.Contains(startDate.DayOfWeek));

                bool overlap = false;
                foreach (var dates in vacationDictionary.Values)
                {
                    if (dates.Any(d => (d >= startDate.AddDays(-3) && d <= startDate.AddDays(vacationLength + 2))))
                    {
                        overlap = true;
                        break;
                    }
                }

                if (!overlap)
                {
                    for (int i = 0; i < vacationLength; i++)
                    {
                        DateTime vacationDay = startDate.AddDays(i);
                        if (workingDays.Contains(vacationDay.DayOfWeek))
                        {
                            vacationDictionary[employee].Add(vacationDay);
                        }
                    }

                    remainingDays -= vacationLength;
                }
            }
        }

        Console.WriteLine("Распределение отпусков сотрудников:");
        foreach (var employee in employees)
        {
            Console.WriteLine($"\n{employee}:");
            var sortedDates = vacationDictionary[employee].OrderBy(d => d).ToList();
            foreach (var date in sortedDates)
            {
                Console.WriteLine(date.ToString("dd MMMM yyyy, dddd"));
            }
        }
    }
}
