using System.Drawing;

namespace AntDesignGameFramework;

public class Display
{
    public Size Size { get; set; }

    public bool IsOutOfRange(RectTransform rect)
    {
        int x = 0;
        int y = 0;

        bool isInRage = (rect.X < x + Size.Width)
            && (x < rect.X + rect.Width)
            && (rect.Y < y + Size.Height)
            && (y < rect.Y + rect.Height);

        return !isInRage;
    }
}
