namespace JsonLibrary
{
    /// <summary>
    /// Интерфейс для работы с представлением JSON объектов в программе
    /// </summary>
    public interface IJSONObject
    {
        /// <summary>
        /// Получает все имена полей объекта
        /// </summary>
        /// <returns>Возвращает IEnumerable (строк) имен</returns>
        IEnumerable<string> GetAllFields();

        /// <summary>
        /// Получает значение поля объекта по имени
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение поля объекта</returns>
        string GetField(string fieldName);

        /// <summary>
        /// Устанавливает значение поля объекта по имени
        /// </summary>
        /// <param name="fieldName">Имя изменяемого поля</param>
        /// <param name="value">Новое значение поля</param>
        void SetField(string fieldName, string value);
    }
}
