using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrossWord
{
    public static class CanvasExt
    {
        public static void DrawQuarter(this Canvas canvas, bool[,] board, int rectSize, int space, 
            bool halfWidth, bool halfHeight, bool isShadowed)
        {
            for (int j = 0; j < board.GetLength(1); j++) // height
            {
                for (int i = 0; i < board.GetLength(0); i++) // width
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Height = (j == 0 && halfHeight) ? rectSize / 2 : rectSize,
                        Width = (i == 0 && halfWidth) ? rectSize / 2 : rectSize,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                    };

                    rectangle.Fill = isShadowed
                        ? board[i, j] ? Brushes.White : Brushes.LightGray
                        : board[i, j] ? Brushes.White : Brushes.Aquamarine;
                    canvas.Children.Add(rectangle);

                    Canvas.SetLeft(rectangle, i * (rectSize + space) - (halfWidth && i > 0 ? 0.5 * rectSize : 0));
                    Canvas.SetTop(rectangle, j * (rectSize + space) - (halfHeight && j > 0 ? 0.5 * rectSize : 0));
                }
            }
        }
    }
}
