/* Ковалев Иван Андреевич
 * БПИ2410, 1 подгруппа
 * Проект 3.1, вариант 8 */

using JsonLibrary;
using VisualLibrary;

//Пространство имен консольного приложения
namespace ConsoleApp
{
    /// <summary>
    /// Основной класс программы
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Главный метод программы
        /// </summary>
        private static void Main()
        {
            //Список посетителей - используется как данные программы
            List<Visitor> data = [];

            //Цикл, повторяющий решение до тех пор, пока пользователь сам его не закончит
            while (true)
            {
                //Показ меню
                ConsoleOutputs.ShowMenu(data.Count == 0);

                //Если действие выполнено успешно, то выводит сообщение об этом
                if (Menu.ChooseMenuItem(ref data))
                {
                    ConsoleOutputs.PrintMessage("Операция успешно выполнена!", ConsoleColor.Green);
                }

                ConsoleOutputs.PrintMessage("Чтобы вернуться в главное меню, нажмите TAB (если хотите вернуться и очистить консоль - нажмите ESC)", ConsoleColor.White);

                //Возвращает пользователя в главное меню с очисткой (или без) консоли
                ConsoleKeyInfo key = Console.ReadKey(true);
                while (key.Key is not ConsoleKey.Escape and not ConsoleKey.Tab)
                {
                    key = Console.ReadKey(true);
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                }
            }
        }

    }

}
