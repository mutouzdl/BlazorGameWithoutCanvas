using Microsoft.AspNetCore.Components;

namespace FightGame;

/// <summary>
/// 参考来源：https://github.com/aventius-software/BlazorGE
/// </summary>
public partial class GraphicAssets : ComponentBase
{
    [Inject]
    protected IGraphicAssetService GraphicAssetService { get; set; }


    protected List<GraphicAsset> _assets = new();


    #region Override Methods

    protected override void OnInitialized()
    {
        GraphicAssetService.RegisterOnLoadGraphicAssetHandler(LoadGraphicAsset);
        GraphicAssetService.RegisterOnReleaseGraphicAssetHandler(ReleaseGraphicAsset);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// When the specified asset has been loaded, this is called
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    protected async Task OnLoadGraphicAsset(string uid)
    {
        // Try and find this sprite sheet
        var assets = _assets.Where(asset => asset.Uid == uid);

        // Does it exist? 
        if (assets.Any())
        {
            // Yes, get it...
            var asset = assets.Single();
            asset.IsLoaded = true;

            // Is there an 'onload' method to call?
            if (asset.OnLoadAsync is not null)
            {
                // Yes, call it and pass the current time of when the image was loaded
                await InvokeAsync(async () => await asset.OnLoadAsync(DateTime.Now));
            }
        }
    }

    #endregion
    public GraphicAsset LoadGraphicAsset(string url)
    {
        var asset = _assets.SingleOrDefault(t => t.Url == url);

        if (asset != null)
        {
            return asset;
        }

        asset = new GraphicAsset { Url = url };
        _assets.Add(asset);
        StateHasChanged();

        return asset;
    }

    public void ReleaseGraphicAsset(GraphicAsset graphicAsset)
    {
        var asset = _assets.SingleOrDefault(t => t.Uid == graphicAsset.Uid);

        if (asset != null)
        {
            _assets.Remove(asset);
            StateHasChanged();
        }
    }
}
