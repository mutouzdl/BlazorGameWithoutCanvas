using Microsoft.AspNetCore.Components;

namespace AntDesignGame;

public partial class Bullet
{
    [Parameter]
    public BulletConfig? BulletConfig { get; set; }

    private double _left = 0;

    public void Logic(double deltaTime)
    {
        if (BulletConfig == null)
        {
            return;
        }

        _left -= BulletConfig.Speed * deltaTime;

        StateHasChanged();
    }
}