namespace JsonLibrary
{
    /// <summary>
    /// Статический класс, помогающий перенаправлять потоки ввода/вывода
    /// </summary>
    public static class ConsoleStream
    {
        /// <summary>
        /// Перенаправляет поток ввода в файл/консоль
        /// </summary>
        /// <param name="inputWay">Способ ввода данных: консоль или файл</param>
        /// <param name="path">Путь к файлу, из которого нужно прочитать данные</param>
        public static void SetInputStream(string inputWay, string? path)
        {
            if (inputWay == "file" && path != null)
            {
                StreamReader @in = new(path, Console.InputEncoding);
                Console.SetIn(@in);
            }
        }

        /// <summary>
        /// Перенаправляет поток вывода в файл/консоль
        /// </summary>
        /// <param name="outputWay">Способ вывода данных: консоль или файл</param>
        /// <param name="path">Путь к файлу, в который нужно записать данные</param>
        /// <param name="fileName">Имя создаваемого/перезаписываемого файла</param>
        public static void SetOutputStream(string outputWay, string? path, string? fileName)
        {
            if (outputWay == "file" && path != null && fileName != null)
            {
                StreamWriter @out = new(path + Path.DirectorySeparatorChar + fileName, false, Console.OutputEncoding);
                Console.SetOut(@out);
            }
        }

        /// <summary>
        /// Устанавливает поток ввода по умолчанию
        /// </summary>
        public static void SetDefaultInputStream()
        {
            Console.In.Close();
            Console.SetIn(new StreamReader(Console.OpenStandardInput(), Console.InputEncoding));
        }

        /// <summary>
        /// Устанавливает поток вывода по умолчанию
        /// </summary>
        public static void SetDefaultOutputStream()
        {
            Console.Out.Close();
            StreamWriter standardOut = new(Console.OpenStandardOutput(), Console.OutputEncoding)
            {
                AutoFlush = true
            };
            Console.SetOut(standardOut);
        }

    }
}
