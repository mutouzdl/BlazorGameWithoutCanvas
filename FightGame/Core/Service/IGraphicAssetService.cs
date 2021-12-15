namespace FightGame;

public interface IGraphicAssetService
{
    public GraphicAsset LoadGraphicAsset(string url);
    public void ReleaseGraphicAsset(GraphicAsset graphicAsset);
    public void RegisterOnLoadGraphicAssetHandler(Func<string, GraphicAsset> eventHandler);
    public void RegisterOnReleaseGraphicAssetHandler(Action<GraphicAsset> eventHandler);
}
