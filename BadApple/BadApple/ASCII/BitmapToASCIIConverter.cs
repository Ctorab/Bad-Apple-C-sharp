using System.Drawing;

namespace BadApple.ASCII
{
    internal static class BitmapToASCIIConverter
    {
        private const double WIDTH_OFFSET = 1.5;

        public static char[] ConvertBitmapToASCII(Bitmap bitmap, string asciiTable)
        {
            float stepCharsSize = 255 / (asciiTable.Length - 1);

            bitmap = ResizeBitmap(bitmap);

            int height = bitmap.Height;
            int width = bitmap.Width;

            char[] asciiResult = new char[bitmap.Height * bitmap.Width];            

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    var pixelIndex = (x + y * width);

                    Color color;
                    lock (bitmap) color = bitmap.GetPixel(x, y);

                    int avg = (color.R + color.G + color.B) / 3;

                    int mapIndex = (int)Math.Floor(avg / stepCharsSize);

                    asciiResult[pixelIndex] = asciiTable[mapIndex];
                }
            });

            return asciiResult;
        }

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            var newBitmapHeight = height / WIDTH_OFFSET * consoleWidth / width;

            if (width > consoleWidth || height > newBitmapHeight)
                bitmap = new(bitmap, new Size(consoleWidth, (int)newBitmapHeight));

            if(bitmap.Height > consoleHeight)
                bitmap = new(bitmap, new Size(bitmap.Width, consoleHeight));

            return bitmap;
        }
    }
}
