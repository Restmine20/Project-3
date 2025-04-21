using System.Text.RegularExpressions;

namespace JsonLibrary
{
    /// <summary>
    /// СТруктура взаимоотношений между посетителями
    /// </summary>
    /// <param name="status">Статус взаимоотношений - дружба или неприязнь</param>
    /// <param name="firstId">ID первого посетителя</param>
    /// <param name="secondId">ID второго посетителя</param>
    /// <param name="firstLabel">LABEL первого посетителя</param>
    /// <param name="secondLabel">LABEL второго посетителя</param>
    public struct Relationship (string status, string firstId, string secondId, string? firstLabel, string? secondLabel) : IJSONObject
    {
        /// <summary>
        /// Словарь, хранящий все данные структуры
        /// </summary>
        private Dictionary<string, string> fields = new()
        {
            { "status", (status == "befriend") ? "Дружба" : "Недоверие"},
            {"firstId", firstId},
            {"secondId", secondId},
            {"firstLabel", firstLabel ?? ""},
            {"secondLabel", secondLabel ?? ""}
        };

        /// <summary>
        /// Реализация метода интерфейса
        /// </summary>
        /// <returns>Все названия ключей словаря</returns>
        public IEnumerable<string> GetAllFields()
        {
            return ["status", "firstId", "secondId", "firstLabel", "secondId", "firstLabel", "secondLabel"];
        }

        /// <summary>
        /// Реализация метода интерфейса
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение поля по имени</returns>
        public string GetField(string fieldName)
        {
            return fields[fieldName];
        }

        /// <summary>
        /// Реализация метода интерфейса
        /// </summary>
        /// <param name="fieldName">Имя изменяемого поля</param>
        /// <param name="value">Значение изменяемого поля</param>
        public void SetField(string fieldName, string value)
        { 
            fields[fieldName] = value;
        }

        /// <summary>
        /// Метод создает по списку посетителей список всех существующих между ними взаимоотношений
        /// </summary>
        /// <param name="data">Список посетителей</param>
        /// <returns>Список объектов Relationship</returns>
        public static List<Relationship> GetRelationships(List<Visitor> data)
        {
            List<Relationship> relationshipsData = [];
            foreach (Visitor visitor in data)
            {
                string xexts = visitor.GetField("xexts");
                //Эта регулярка - формат ключей в xexts, чтобы по ним понимать связи
                Regex pattern = new(@"\w+\.\w+\.\w+");

                MatchCollection matches = pattern.Matches(xexts);
                foreach (Match match in matches)
                {
                    string status = match.Value[..match.Value.IndexOf('.')];
                    string firstId = match.Value[(match.Value.IndexOf('.') + 1)..match.Value.LastIndexOf('.')];
                    string secondId = match.Value[(match.Value.LastIndexOf('.') + 1)..];
                    string? firstLabel = Visitor.GetLabelById(firstId, data), secondLabel = Visitor.GetLabelById(secondId, data);
                    //Если подобной связи не было - закидываем в список
                    Relationship relationship = new(status, firstId, secondId, firstLabel, secondLabel);
                    if (!relationshipsData.Contains(relationship))
                    {
                        relationshipsData.Add(relationship);
                    }
                }
            }
            return relationshipsData;
        }

        /// <summary>
        /// Перегрузка наследуемого метода
        /// </summary>
        /// <returns>Хэш объекта</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Перегрузка наследуемого метода
        /// </summary>
        /// <param name="obj">Объект для сравения</param>
        /// <returns>Если объекты представляют одну и ту же взаимосвязь - true, иначе - false</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Relationship))
            {
                return false;
            }
            Relationship that = (Relationship)obj;
            return (GetField("status") == that.GetField("status")) && (GetField("firstId") == that.GetField("firstId") || GetField("firstId") == that.GetField("secondId")) && (GetField("secondId") == that.GetField("secondId") || GetField("secondId") == that.GetField("secondId"));
        }

        /// <summary>
        /// Оператор эквивалентен Equals()
        /// </summary>
        /// <param name="left">Первый объект</param>
        /// <param name="right">Второй объект</param>
        /// <returns>Равенство - true, не равенство - false</returns>
        public static bool operator == (Relationship left, Relationship right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Оператор эквивалентен отрицанию Equals()
        /// </summary>
        /// <param name="left">Первый объект</param>
        /// <param name="right">Второй объект</param>
        /// <returns>Не равенство - true, равенство - false </returns>
        public static bool operator != (Relationship left, Relationship right)
        {
            return !left.Equals(right);
        }
    }
}
