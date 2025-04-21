using JsonLibrary;
namespace VisualLibrary
{
    /// <summary>
    /// Класс, красиво выводящий разного формата сообщения в консоль
    /// </summary>
    public static class ConsoleOutputs
    {
        /// <summary>
        /// Выводит сообщение в консоль
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        /// <param name="endLine">true - если нужно перейти на следующую строку, false -  если нет</param>
        public static void PrintMessage(string message, ConsoleColor color = ConsoleColor.Gray, bool endLine = true)
        {
            Console.ForegroundColor = color;
            if (endLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит таблицу данных (пункт меню 4)
        /// </summary>
        /// <param name="data">Список взаимосвязей между посетителями для составления таблицы</param>
        public static void FormatOutput(List<Relationship> data)
        {
            string[] header = ["Статус", "ID посетителя", "Имя посетителя"];
            int[] sizesOfCells = [header[0].Length, header[1].Length, header[2].Length];
            for (int i = 0; i < data.Count; i++)
            {
                sizesOfCells[0] = Math.Max(data[i].GetField("status").Length, sizesOfCells[0]);
                sizesOfCells[1] = Math.Max(Math.Max(data[i].GetField("firstId").Length, data[i].GetField("secondId").Length), sizesOfCells[1]);
                sizesOfCells[2] = Math.Max(Math.Max(data[i].GetField("firstLabel").Length, data[i].GetField("secondLabel").Length), sizesOfCells[2]);
            }

            int sumOfSizes = 3;
            foreach (int x in sizesOfCells)
            {
                sumOfSizes += x;
            }
            for (int i = 0; i < 3; i++)
            {
                PrintMessage(header[i] + new string(' ', sizesOfCells[i] - header[i].Length), ConsoleColor.Magenta, false);
                PrintMessage("|", ConsoleColor.Gray, false);
            }
            PrintMessage("", ConsoleColor.Gray);
            PrintMessage(new string('_', sumOfSizes));

            for (int i = 0; i < data.Count; i++)
            {
                PrintMessage(data[i].GetField("status") + new string(' ', sizesOfCells[0] - data[i].GetField("status").Length), (data[i].GetField("status") == "Дружба") ? ConsoleColor.Green : ConsoleColor.Red, false);
                PrintMessage("|", ConsoleColor.Gray, false);
                PrintMessage(data[i].GetField("firstId") + new string(' ', sizesOfCells[1] - data[i].GetField("firstId").Length), ConsoleColor.Yellow, false);
                PrintMessage("|", ConsoleColor.Gray, false);
                PrintMessage(data[i].GetField("firstLabel") + new string(' ', sizesOfCells[2] - data[i].GetField("firstLabel").Length), ConsoleColor.Cyan, false);
                PrintMessage("|", ConsoleColor.Gray);
                PrintMessage(new string(' ', sizesOfCells[0]), ConsoleColor.Gray, false);
                PrintMessage("|", ConsoleColor.Gray, false);
                PrintMessage(data[i].GetField("secondId") + new string(' ', sizesOfCells[1] - data[i].GetField("secondId").Length), ConsoleColor.Yellow, false);
                PrintMessage("|", ConsoleColor.Gray, false);
                PrintMessage(data[i].GetField("secondLabel") + new string(' ', sizesOfCells[2] - data[i].GetField("secondLabel").Length), ConsoleColor.Cyan, false);
                PrintMessage("|", ConsoleColor.Gray);
                PrintMessage(new string('_', sumOfSizes));

            }

        }

        /// <summary>
        /// Показывает меню в консоли
        /// </summary>
        /// <param name="IsEmpty">Если список пустой, то 1-ый пункт меню просит ввести данные, если нет - то изменить</param>
        public static void ShowMenu(bool IsEmpty)
        {
            ConsoleOutputs.PrintMessage("Введите номер пункта меню для запуска действия:", ConsoleColor.Yellow);

            if (IsEmpty)
            {
                ConsoleOutputs.PrintMessage("1. ", ConsoleColor.Green, false);
                ConsoleOutputs.PrintMessage("Загрузить данные из консоли/файла");
            }
            else
            {
                ConsoleOutputs.PrintMessage("1. ", ConsoleColor.Green, false);
                ConsoleOutputs.PrintMessage("Изменить входные данные");

            }
            ConsoleOutputs.PrintMessage("2. ", ConsoleColor.Green, false);
            ConsoleOutputs.PrintMessage("Отфильтровать данные");

            ConsoleOutputs.PrintMessage("3. ", ConsoleColor.Green, false);
            ConsoleOutputs.PrintMessage("Отсортировать данные");

            ConsoleOutputs.PrintMessage("4. ", ConsoleColor.Green, false);
            ConsoleOutputs.PrintMessage("Составить таблицу взаимоотношений посетителей");

            ConsoleOutputs.PrintMessage("5. ", ConsoleColor.Green, false);
            ConsoleOutputs.PrintMessage("Построить граф взаимоотношений посетителей");

            ConsoleOutputs.PrintMessage("6. ", ConsoleColor.Green, false);
            ConsoleOutputs.PrintMessage("Вывести данные в консоль/файл");

            ConsoleOutputs.PrintMessage("7. ", ConsoleColor.Green, false);
            ConsoleOutputs.PrintMessage("Завершить программу");
        }
    }
}
