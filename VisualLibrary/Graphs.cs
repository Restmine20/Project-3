using JsonLibrary;
using SkiaSharp;
namespace VisualLibrary
{
    /// <summary>
    /// Класс, рисующий граф (пункт меню 5)
    /// </summary>
    public static class Graphs
    {
        /// <summary>
        /// Основной метод рисования графа
        /// </summary>
        /// <param name="relationshipsData">Список взаимосвязей между посетителями</param>
        /// <param name="outputPath">Путь к выходному файлу</param>
        /// <param name="fileName">Имя выходного файла</param>
        /// <param name="inputPath">Путь к директории с изображениями для графа</param>
        public static void DrawGraph(List<Relationship> relationshipsData, string outputPath, string fileName, string inputPath)
        {
            List<string> vertexesData = GetVertexes(relationshipsData);
            int height = (vertexesData.Count * 306) + 120, width = (vertexesData.Count * 256) + 120;

            Dictionary<string, (int, int)> coordsOfVertexes = GetCoords(vertexesData, height, width);

            SKBitmap bitmap = new(width, height);
            using SKCanvas canvas = new(bitmap);
            canvas.Clear(SKColors.White);
            using FileStream stream = new(outputPath + Path.DirectorySeparatorChar + fileName, FileMode.Create, FileAccess.Write);

        
            DrawRelationships(canvas, relationshipsData, coordsOfVertexes);
            DrawIcon(canvas, coordsOfVertexes, inputPath);
            DrawId(canvas, coordsOfVertexes);

            using SKImage image = SKImage.FromBitmap(bitmap);
            using SKData encodedImage = image.Encode();
            encodedImage.SaveTo(stream);

        }

        /// <summary>
        /// Получает список неповторяющихся вершин для графа
        /// </summary>
        /// <param name="relationshipsData">Список взаимосвязей между посетителями</param>
        /// <returns>Список неповторяющихся вершин для графа</returns>
        public static List<string> GetVertexes(List<Relationship> relationshipsData)
        {
            List<string> vertexes = [];
            foreach (Relationship relationship in relationshipsData)
            {
                string firstId = relationship.GetField("firstId");
                string secondId = relationship.GetField("secondId");
                if (!vertexes.Contains(firstId))
                {
                    vertexes.Add(firstId);
                }
                if (!vertexes.Contains(secondId))
                {
                    vertexes.Add(secondId);
                }
            }
            return vertexes;
        }

        /// <summary>
        /// Сопоставляет каждой вершине графа координаты так, что граф был "кольцом"
        /// </summary>
        /// <param name="vertexesData">Список вершин (ID некоторых посетителей)</param>
        /// <param name="height">Высота окна для рисования</param>
        /// <param name="width">Ширина окна для рисования</param>
        /// <returns>Словарь, сопоставляющий ID - координаты</returns>
        public static Dictionary<string, (int, int)> GetCoords(List<string> vertexesData, int height, int width)
        {
            int radius = Math.Abs((width / 2) - 188);
            double phi = 0;
            Dictionary<string, (int, int)> result = [];
            for (int i = 0; i < vertexesData.Count; i++)
            {
                //каждой вершине даем координаты такие, чтобы в системе координат в центре холста вся их совокупность давала "кольцо"
                int rawX = (int)Math.Round(radius*Math.Cos(phi));
                int rawY = (int)Math.Round(radius*Math.Sin(phi));
                //Сохраняем координаты уже в другой системе, чтобы SK все рисовала правильно (с центром слева сверху)
                result[vertexesData[i]] = ((width/2) + rawX, (height/2) - rawY);
                phi += 2*Math.PI / vertexesData.Count;
            }
            return result;
        }

        /// <summary>
        /// В случае, если иконка не была найдена, используется этот метод для рисования черного квадрата
        /// </summary>
        /// <param name="canvas">Холст для рисования</param>
        /// <param name="startX">Центр квадрата по X</param>
        /// <param name="startY">Центр квадрата по Y</param>
        public static void DrawSquare(SKCanvas canvas, int startX, int startY)
        {
            using SKPaint paint = new();
            paint.Color = SKColors.Black;
            SKRect square = new(startX-128, startY-128, startX + 128, startY + 128);
            canvas.DrawRect(square, paint);
        }

        /// <summary>
        /// Рисует линии связей между посетителями
        /// </summary>
        /// <param name="canvas">Холст для рисования</param>
        /// <param name="relationshipsData">Список связей между посетителями</param>
        /// <param name="coordsOfVertexes">Словарь ID-координат</param>
        public static void DrawRelationships(SKCanvas canvas, List<Relationship> relationshipsData, Dictionary<string, (int, int)> coordsOfVertexes)
        {
            foreach (Relationship relationship in relationshipsData)
            {
                int firstX = coordsOfVertexes[relationship.GetField("firstId")].Item1, firstY = coordsOfVertexes[relationship.GetField("firstId")].Item2;
                int secondX = coordsOfVertexes[relationship.GetField("secondId")].Item1, secondY = coordsOfVertexes[relationship.GetField("secondId")].Item2;

                string status = relationship.GetField("status");
                using SKPaint paint = new();
                paint.Color = (status == "Дружба") ? SKColors.Green : SKColors.Red;
                paint.StrokeWidth = 12;
                canvas.DrawLine(firstX, firstY, secondX, secondY, paint);
            }
        }

        /// <summary>
        /// Подписывает иконки с помощью ID
        /// </summary>
        /// <param name="canvas">Холст для рисования</param>
        /// <param name="coordsOfVertexes">Словарь ID-координат</param>
        public static void DrawId(SKCanvas canvas, Dictionary<string, (int, int)> coordsOfVertexes)
        {
            foreach (KeyValuePair<string, (int, int)> item in coordsOfVertexes)
            {
                using SKFont font = new(SKTypeface.FromFamilyName("Times New Roman", new SKFontStyle(0, 30, SKFontStyleSlant.Upright)), 75);
                using SKPaint paintText = new();
                paintText.Color = SKColors.Black;
                canvas.DrawText(item.Key, item.Value.Item1 - 128, item.Value.Item2 + 185, font, paintText);
            }
        }

        /// <summary>
        /// Рисует сами иконки персонажей
        /// </summary>
        /// <param name="canvas">Холст для рисования</param>
        /// <param name="coordsOfVertexes">Словарь ID-координат</param>
        /// <param name="inputPath">Путь к директориии с картинками</param>
        public static void DrawIcon(SKCanvas canvas, Dictionary<string, (int, int)> coordsOfVertexes, string inputPath)
        {
            foreach(KeyValuePair<string, (int, int)> item in coordsOfVertexes)
            {
               
                SKImage image = SKImage.FromEncodedData(inputPath + Path.DirectorySeparatorChar + item.Key + ".png");
                try 
                {
                    canvas.DrawImage(image, item.Value.Item1-128, item.Value.Item2-128);
                }
                catch
                {
                    DrawSquare(canvas, item.Value.Item1, item.Value.Item2);
                }

            }
        }
    }
}
