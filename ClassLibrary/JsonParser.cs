using System.Text.RegularExpressions;
namespace JsonLibrary
{
    /// <summary>
    /// Класс, представляющий методы для чтения и записи JSON файлов
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Метод считывает JSON файл и возвращает созданный список структур
        /// </summary>
        /// <typeparam name="T">В моем варианте - посетитель, в общем случае - структура, реализующая интерфейс и имеющая пустой конструктор</typeparam>
        /// <returns>Список посетителей</returns>
        /// <exception cref="FormatException">Исключение означает невалидность файла</exception>
        public static List<T> ReadJson<T>() where T: IJSONObject, new()
        {
            List<string> rawData = [];

            Regex spaces = new(@"\s");

            string rawJsonObejct = "";

            //Считаем скобочки, проверяем цитаты
            int figure_scopes_opened = 0;
            int figure_scopes_closed = 0;

            bool is_quotes_open = false;

            int symb = Console.Read();

            //Основной цикл ввода - по сути автомат
            while (symb != -1 && (figure_scopes_closed < figure_scopes_opened || figure_scopes_opened == 0))
            {
                if (symb == '"')
                {
                    is_quotes_open = !is_quotes_open;
                }

                if ((char)symb == '{')
                {
                    figure_scopes_opened += 1;
                }

                if (figure_scopes_opened - figure_scopes_closed >= 2)
                {
                    if (is_quotes_open || !spaces.IsMatch(((char)symb).ToString()))
                    {
                        rawJsonObejct += ((char)symb).ToString();
                    }
                }

                if ((char)symb == '}')
                {
                    figure_scopes_closed += 1;

                }
                
                if (figure_scopes_opened > 1 && figure_scopes_opened - figure_scopes_closed == 1 && rawJsonObejct != "")
                {
                    rawData.Add(rawJsonObejct);
                    rawJsonObejct = "";
                }
                symb = Console.Read();
            }

            //Небольшая проверка корректности
            if (is_quotes_open || figure_scopes_closed != figure_scopes_opened)
            {
                throw new FormatException();
            }
            List<T> result = [];
            for (int i = 0; i < rawData.Count; i++)
            {
                result.Add(JsonProcessing.GenerateVisitor<T>(rawData[i]));
            }
            
            return result;
        }

        /// <summary>
        /// Записывает все объекты из списка в JSON файл
        /// </summary>
        /// <typeparam name="T">В моем варианте - посетитель, в общем случае - структура, реализующая интерфейс и имеющая пустой конструктор</typeparam>
        /// <param name="data">Список посетителей, который нужно записать в JSON файл</param>
        public static void WriteJson<T>(List<T> data) where T : IJSONObject, new()
        {
            //Открываем файл
            Console.Write("{\n\t\"elements\":\n\t[");
            int countOfOpenedScopes = 2;

            //Основной цикл вывода
            for (int j = 0; j < data.Count; j++) 
            {
                T jsonObject = data[j];
                string json = JsonProcessing.GenerateText(jsonObject);
                bool isQuoteOpen = false;

                //Идем по строковому представлению каждого объекта, немного шаманим с символами, чтобы получть красивый вывод
                for (int i = 0; i < json.Length; i++)
                {
                    if (!isQuoteOpen) 
                    {
                        if (json[i] is '{' or '[')
                        {
                            Console.Write('\n');
                            Console.Write(new string('\t', countOfOpenedScopes));
                            countOfOpenedScopes++;
                        }
                        else if (i > 0 && (json[i - 1] is ',' or '{' or '['))
                        {
                            Console.Write('\n');
                            Console.Write(new string('\t', countOfOpenedScopes));
                        }
                        if (json[i] is ']' or '}')
                        {
                            countOfOpenedScopes--;
                            Console.Write('\n');
                            Console.Write(new string('\t', countOfOpenedScopes));
                        }
                    }
                   
                    if (json[i] == '"')
                    {
                        isQuoteOpen = !isQuoteOpen;
                    }
                    Console.Write(json[i]);
                }
                if (j < data.Count - 1)
                {
                    Console.Write(',');
                }
            }
            //Закрываем файл
            Console.WriteLine("\n\t]\n}");
        }
    }
}
