using System.Text.RegularExpressions;

namespace JsonLibrary
{
    /// <summary>
    /// Вспомогательный, для JsonParser, класс, который преобразует посетителя в его строковое представление или наоборот
    /// </summary>
    internal static class JsonProcessing
    {
        /// <summary>
        /// Принимает строковое представление JSON объекта и конвертирует его в T объект
        /// </summary>
        /// <typeparam name="T">В моем варианте - посетитель, в общем случае - структура, реализующая интерфейс и имеющая пустой конструктор</typeparam>
        /// <param name="raw_object">Строковое представление JSON объекта</param>
        /// <returns>Созданная структура (посетитель)</returns>
        /// <exception cref="FormatException">Исклчение означает, что переданная строка не является объектом</exception>
        internal static T GenerateVisitor<T>(string raw_object) where T: IJSONObject, new()
        {
            T jsonObject= new();
            IEnumerable<string> fields = jsonObject.GetAllFields();

            foreach (string field in fields)
            {
                //Эта регулярка - представление одного любого поля объекта
                string s = $"\\s*\"{field}\"\\s*:\\s*({{(\\s*\"(.)*?\"\\s*:\\s*(\\[(\\s*({{(\\s*\"(.)*?\"\\s*:\\s*(\"(.)*?\"|\\d),?\\s*)*}}|\\\"(.)*?\"|\\d*),?)*\\]|\"(.)*?\"|\\d),?\\s*)*}}|\"(.)*?\"|\\d*)";
                Regex r = new(s);

                Match x = r.Match(raw_object);
                //Если поле есть, а регулярка ничего не нашла, значит, файл некорректен
                if (x.Value == "" && raw_object.Contains(field))
                {
                    throw new FormatException();
                }
                jsonObject.SetField(field, (x.Value == "") ? x.Value : x.Value[(x.Value.IndexOf(':') + 1)..].Trim(['"', ' ', '\n', '\r', '\t']));
            }

            return jsonObject;
        }

        /// <summary>
        /// Преобразует JSON объект в его строковое представление
        /// </summary>
        /// <typeparam name="T">В моем варианте - посетитель, в общем случае - структура, реализующая интерфейс и имеющая пустой конструктор</typeparam>
        /// <param name="jsonObject">Конвертируемый объект</param>
        /// <returns>Строковое представление JSON объекта</returns>
        internal static string GenerateText<T>(T jsonObject) where T: IJSONObject, new()
        {
            string result = "{";
            //Грубо - проходимся по полям, закидываем строку ключ:значение, возвращаем строку обратно
            foreach (string field in jsonObject.GetAllFields())
            {
                string value = jsonObject.GetField(field);
                if (value != "")
                {
                    if (value[0] != '[' && value[0] != '{' && !char.IsDigit(value[0]))
                    {
                        value = "\"" + value + "\"";
                    }
                    result += "\"" + field + "\":" + value + ",";
                }
            }
            return result[..^1] + "}";
        }
    }
}
