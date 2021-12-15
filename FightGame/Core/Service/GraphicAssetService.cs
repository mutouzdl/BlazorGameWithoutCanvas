namespace FightGame;

/// <summary>
/// 参考来源：https://github.com/aventius-software/BlazorGE
/// </summary>
public class GraphicAssetService : IGraphicAssetService
{
    private event Func<string, GraphicAsset> OnLoadGraphicAssetHandlers;
    private event Action<GraphicAsset> OnReleaseGraphicAssetHandlers;

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public GraphicAsset LoadGraphicAsset(string url)
    {
        return OnLoadGraphicAssetHandlers.Invoke(url);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="graphicAsset"></param>
    public void ReleaseGraphicAsset(GraphicAsset graphicAsset)
    {
        OnReleaseGraphicAssetHandlers.Invoke(graphicAsset);
    }

    public void RegisterOnLoadGraphicAssetHandler(Func<string, GraphicAsset> eventHandler)
    {
        if (OnLoadGraphicAssetHandlers is not null) OnLoadGraphicAssetHandlers -= eventHandler;
        OnLoadGraphicAssetHandlers += eventHandler;
    }

    public void RegisterOnReleaseGraphicAssetHandler(Action<GraphicAsset> eventHandler)
    {
        if (OnReleaseGraphicAssetHandlers is not null) OnReleaseGraphicAssetHandlers -= eventHandler;
        OnReleaseGraphicAssetHandlers += eventHandler;
    }
}
