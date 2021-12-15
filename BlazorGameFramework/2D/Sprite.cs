using System.Drawing;

using Microsoft.AspNetCore.Components;

namespace BlazorGameFramework;

public class Sprite
{
    public Size Size { get; set; }
    public ElementReference SpriteSheet { get; set; }
}
