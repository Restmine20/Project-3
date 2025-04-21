using JsonLibrary;
using System.Security;
using VisualLibrary;

namespace ConsoleApp
{
    /// <summary>
    /// Основной класс меню
    /// </summary>
    internal static class Menu
    {
        /// <summary>
        /// Метод предоставляет возможность выбрать действие в меню
        /// </summary>
        /// <param name="data">Список данных посетителей, который может быть изменени этим методом</param>
        /// <returns>Успешно ли проведено действие в выбранном пункте меню</returns>
        internal static bool ChooseMenuItem(ref List<Visitor> data)
        {
            bool isSuccess = false;
            ConsoleKeyInfo key = Console.ReadKey(true);
            bool isEmpty = data.Count == 0;
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    isSuccess = MenuItem1(out data);
                    break;
                case ConsoleKey.D2:

                    if (isEmpty) { ConsoleOutputs.PrintMessage("Сначала введите данные", ConsoleColor.Red); break; }
                    isSuccess = MenuItem2(ref data);
                    break;
                case ConsoleKey.D3:

                    if (isEmpty) { ConsoleOutputs.PrintMessage("Сначала введите данные", ConsoleColor.Red); break; }
                    isSuccess = MenuItem3(data);
                    break;
                case ConsoleKey.D4:

                    if (isEmpty) { ConsoleOutputs.PrintMessage("Сначала введите данные", ConsoleColor.Red); break; }
                    isSuccess = MenuItem4(data);
                    break;
                case ConsoleKey.D5:

                    if (isEmpty) { ConsoleOutputs.PrintMessage("Сначала введите данные", ConsoleColor.Red); break; }
                    isSuccess = MenuItem5(data);
                    break;
                case ConsoleKey.D6:

                    if (isEmpty) { ConsoleOutputs.PrintMessage("Сначала введите данные", ConsoleColor.Red); break; }
                    isSuccess = MenuItem6(data);
                    break;

                case ConsoleKey.D7:

                    Environment.Exit(0);
                    break;

                default:

                    ConsoleOutputs.PrintMessage("Выберете корректный пункт меню (число от 1 до 7)", ConsoleColor.Red);
                    break;
            }
            return isSuccess;
        }

        /// <summary>
        /// Выполняет ввод данных
        /// </summary>
        /// <param name="data">Список данных посетителей</param>
        /// <returns>Успешно ли проведено действие в этом пункте меню</returns>
        private static bool MenuItem1(out List<Visitor> data)
        {
            string inputWay = GetInputFromUser("Если хотите считать данные из консоли, введите \"console\", если из файла - \"file\".", (string x) => x is "console" or "file");
            string? path = null;
            if (inputWay == "file")
            {
                path = GetInputFromUser("Введите корректный путь до доступного файла, откуда надо считать данные", File.Exists);
            }
            //Ловим разные ошибки
            try
            {
                ConsoleStream.SetInputStream(inputWay, path);
                data = JsonParser.ReadJson<Visitor>();
            }
            catch (SecurityException)
            {
                data = [];
                ConsoleOutputs.PrintMessage("Нет прав для чтения из указанного файла", ConsoleColor.Red);
                return false;
            }
            catch (FormatException)
            {
                data = [];
                ConsoleOutputs.PrintMessage("Указанный JSON файл невалидный или не соотвествует структуре файла варианта", ConsoleColor.Red);
                return false;
            }
            catch (IOException)
            {
                data = [];
                ConsoleOutputs.PrintMessage("Произошла ошибка ввода", ConsoleColor.Red);
                return false;
            }
            finally
            {
                ConsoleStream.SetDefaultInputStream();
            }
            return true;
        }

        /// <summary>
        /// Выполняет фильтрацию данных программы
        /// </summary>
        /// <param name="data">Список данных посетителей</param>
        /// <returns>Успешно ли проведено действие в этом пункте меню</returns>
        private static bool MenuItem2(ref List<Visitor> data)
        {
            string nameOfFilteredField = GetInputFromUser("Введите название поля, по которому хотите отфильтровать, из следующего списка:\n" + string.Join(", ", Visitor.GetSortableOrFilterableFields()), (x) => Array.IndexOf(Visitor.GetSortableOrFilterableFields(), x) != -1);
            string inputLine = GetInputFromUser("Вводите нужные для фильтрации значения выбранного поля через запятую (ENTER означает конец ввода)", (x) => true);
            List<Visitor> result = [];
            string[] valuesOfFilteredField = inputLine.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //Фильтруем
            result = Visitor.FilterData(data, valuesOfFilteredField, nameOfFilteredField);
            ConsoleOutputs.PrintMessage($"Вы фильтровали по значениям поля {nameOfFilteredField}; найдено объектов: {result.Count}/{data.Count}", ConsoleColor.Magenta);
            

            data = result;
            return true;
        }

        /// <summary>
        /// Выполняет сортировку данных программы
        /// </summary>
        /// <param name="data">Список данных посетителей</param>
        /// <returns>Успешно ли проведено действие в этом пункте меню</returns>
        private static bool MenuItem3(List<Visitor> data)
        {
            string nameOfSortedField = GetInputFromUser("Введите название поля, по которому хотите отсортировать, из следующего списка:\n" + string.Join(", ", Visitor.GetSortableOrFilterableFields()), (x) => Array.IndexOf(Visitor.GetSortableOrFilterableFields(), x) != -1);
            string sortingWay = GetInputFromUser("Если хотите сортировать по возрастанию, введите \"up\", по убыванию - \"down\"", (x) => x is "up" or "down");

            //Сортируем
            Visitor.SortData(data, nameOfSortedField, sortingWay == "up");

            ConsoleOutputs.PrintMessage($"Вы сортировали по значениям поля {nameOfSortedField}", ConsoleColor.Magenta);
            return true;
        }

        /// <summary>
        /// Строит таблицу взаимосвязей посетителей
        /// </summary>
        /// <param name="data">Список данных посетителей</param>
        /// <returns>Успешно ли проведено действие в этом пункте меню</returns>
        private static bool MenuItem4(List<Visitor> data)
        {
            List<Relationship> relationshipsData = Relationship.GetRelationships(data);

            //Выводим таблицу
            ConsoleOutputs.FormatOutput(relationshipsData);
            return true;
        }

        /// <summary>
        /// Визуализирует связи между посетителями
        /// </summary>
        /// <param name="data">Список данных посетителей</param>
        /// <returns>Успешно ли проведено действие в этом пункте меню</returns>
        private static bool MenuItem5(List<Visitor> data)
        {
            string inputPath = GetInputFromUser("Введите корректный путь до директории, в которой находятся изображения для генерации графа", Directory.Exists); ;
            string outputPath = GetInputFromUser("Введите корректный путь до директории с выходным файлом", Directory.Exists);
            string fileName = GetInputFromUser("Введите корректное название файла (с .png в конце)", (x) => x.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && x.Length >= 5 && x[^4..] == ".png");

            //Ловим разные ошибки
            try
            {
                Graphs.DrawGraph(Relationship.GetRelationships(data), outputPath, fileName, inputPath);
            }
            catch (SecurityException)
            {
                ConsoleOutputs.PrintMessage("Нет разрешения для записи в указанный файл", ConsoleColor.Red);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleOutputs.PrintMessage("Указанный файл может быть только прочитан", ConsoleColor.Red);
                return false;
            }
            catch (IOException)
            {
                ConsoleOutputs.PrintMessage("Произошла ошибка вывода в файл", ConsoleColor.Red);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Выводит данные программы в консоль или в файл
        /// </summary>
        /// <param name="data">Список данных посетителей</param>
        /// <returns>Успешно ли проведено действие в этом пункте меню</returns>
        private static bool MenuItem6(List<Visitor> data)
        {
            string outputWay = GetInputFromUser("Если хотите вывести данные в консоль, введите \"console\", если в файл - \"file\".", (x) => x is "console" or "file");
            string? outputPath = null;
            string? fileName = null;
            if (outputWay == "file")
            {
                outputPath = GetInputFromUser("Введите корректный путь до директории с выходным файлом", Directory.Exists);
                fileName = GetInputFromUser("Введите корректное название файла (с .json в конце)", (x) => x.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && x.Length >= 6 && x[^5..] == ".json");
            }

            //Ловим разные ошибки
            try
            {
                ConsoleStream.SetOutputStream(outputWay, outputPath, fileName);
                JsonParser.WriteJson(data);
                ConsoleStream.SetDefaultOutputStream();
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleOutputs.PrintMessage("Нет прав для записи в указанный файл", ConsoleColor.Red);
                return false;
            }
            catch (SecurityException)
            {
                ConsoleOutputs.PrintMessage("Нет прав для записи в указанный файл", ConsoleColor.Red);
                return false;
            }
            catch (ArgumentException)
            {
                ConsoleOutputs.PrintMessage("Указанный файл недоступен для записи", ConsoleColor.Red);
                return false;
            }
            catch (IOException)
            {
                ConsoleOutputs.PrintMessage("Произошла ошибка ввода", ConsoleColor.Red);
                return false;
            }
            finally
            {
                ConsoleStream.SetDefaultOutputStream();
            }
            return true;
        }

        /// <summary>
        /// Получает от пользователя некоторую строку из консоли
        /// </summary>
        /// <param name="message">Сообщение, которое выводится пользователю</param>
        /// <param name="predicate">Условие, которому должна соответствовать возвращаемая строка</param>
        /// <returns>Возвращаемая строка</returns>
        private static string GetInputFromUser(string message, Predicate<string> predicate)
        {
            string? result;
            do
            {
                ConsoleOutputs.PrintMessage(message, ConsoleColor.Yellow);
                result = Console.ReadLine();
            }
            while (result == null || !predicate(result));
            return result;
        }
    }
}