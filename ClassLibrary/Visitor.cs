namespace JsonLibrary
{
    /// <summary>
    /// Поетитель - представление входных JSON объектов в программе
    /// </summary>
    public struct Visitor(): IJSONObject
    {
        /// <summary>
        /// Словарь, хранящий все данные структуры
        /// </summary>
        private Dictionary<string, string> fields = [];

        public static string[] GetSortableOrFilterableFields()
        {
            return ["id", "label", "desc", "inherits", "decayto", "lifetime", "icon", "audio", "comments"];

        }

        /// <summary>
        /// Реализация метода интерфейса
        /// </summary>
        /// <returns>Все названия ключей словаря</returns>
        public IEnumerable<string> GetAllFields()
        {
            return ["id", "label", "desc", "inherits", "aspects", "decayto", "lifetime", "xtriggers", "xexts", "icon", "audio", "comments"];
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
        /// Ищет в переданном списке объект с нужным id  возвращает его label
        /// </summary>
        /// <param name="id">Искомое id</param>
        /// <param name="data">Список посетителей</param>
        /// <returns>Label искомого объекта</returns>
        public static string? GetLabelById(string id, List<Visitor> data)
        {
            foreach (Visitor visitor in data)
            {
                if (visitor.GetField("id") == id)
                {
                    return visitor.GetField("label");
                }
            }
            return null;
        }

        /// <summary>
        /// Фильтрует переданный список
        /// </summary>
        /// <param name="data">Список для фильтрации</param>
        /// <param name="valuesOfFilteredField">Значения поля фильтрации, по которым отбираются объекты</param>
        /// <param name="nameOfFilteredField">Имя поля фильтрации</param>
        /// <returns>Отфильтрованный список</returns>
        public static List<Visitor> FilterData(List<Visitor> data, string[] valuesOfFilteredField, string nameOfFilteredField)
        {
            List<Visitor> result = [];
            foreach (Visitor visitor in data)
            {
                if (valuesOfFilteredField.Contains(visitor.GetField(nameOfFilteredField)))
                {
                    result.Add(visitor);
                }
            }
            return result;
        }

        /// <summary>
        /// Сортирует переданный список
        /// </summary>
        /// <param name="data">Список для сортировки</param>
        /// <param name="nameOfSortedField">Имя поля для сортировки</param>
        /// <param name="isIncreasing">Направление сортировки</param>
        public static void SortData(List<Visitor> data, string nameOfSortedField, bool isIncreasing)
        {
            Comparison<Visitor> comparison = (Visitor x, Visitor y) => isIncreasing ? string.Compare(x.GetField(nameOfSortedField), y.GetField(nameOfSortedField)) : -string.Compare(x.GetField(nameOfSortedField), y.GetField(nameOfSortedField));
            data.Sort(comparison);
        }
    }
}
