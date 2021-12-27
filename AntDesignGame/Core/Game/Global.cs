using AntDesign.JsInterop;
using AntDesignGameFramework;
using Microsoft.JSInterop;

namespace AntDesignGame;


public class Global
{
    /// <summary>
    /// 游戏上下文
    /// </summary>
    public static GameContext GameContext { get; set; }
    public static IDomEventListener DomEventListener { get; set; }
    public static IJSRuntime JSRuntime { get; set; }
}
