
using System.Numerics;
using AntDesignGameFramework;
using AntDesignGameFramework.Utility;

namespace AntDesignGame;

/// <summary>
/// 直线移动组件
/// </summary>
public class LinerMoveComponent : BaseComponent
{
    public Vector2 Target { get; set; }
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public bool AutoDestroy { get; set; }
    public bool IsMoving { get; private set; } = true;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">目标位置</param>
    /// <param name="direction">方向</param>
    /// <param name="speed">移动速度</param>
    /// <param name="autoDestroy">超出世界边界时，自动销毁对象</param>
    public LinerMoveComponent(GameObject owner, Vector2? target, Vector2? direction, float speed, bool autoDestroy)
    {
        if (target == null && direction == null)
        {
            throw new ArgumentNullException("target and direction cannot both be null.");
        }

        Speed = speed;
        AutoDestroy = autoDestroy;

        Target = target != null ? (Vector2)target : new Vector2(
             (Global.GameContext.Display.Size.Width + owner.Transform.Size.Width) * ((Vector2)direction).X,
             (Global.GameContext.Display.Size.Height + owner.Transform.Size.Height) * ((Vector2)direction).Y
            );

        Direction = direction != null ? (Vector2)direction : new Vector2(
            ((Vector2)target).X - owner.Transform.Position.X > 0 ? 1 : -1,
            ((Vector2)target).Y - owner.Transform.Position.Y > 0 ? 1 : -1);
    }

    public void Pause()
    {
        IsMoving = false;
    }

    public void Resume()
    {
        IsMoving = true;
    }

    public override async ValueTask Update(GameContext game)
    {
        if (Owner.IsDestroy || IsMoving == false)
        {
            return;
        }

        if (game.Display.IsOutOfRange(Owner.Transform.Rect))
        {
            if (AutoDestroy)
            {
                Owner.Destroy();
            }
            return;
        }

        var offsetPos = GetNextOffsetPos(game);
        Owner.Transform.LocalPosition = new Vector2(
            Owner.Transform.LocalPosition.X + offsetPos.X,
            Owner.Transform.LocalPosition.Y + offsetPos.Y);

    }

    private Vector2 GetNextOffsetPos(GameContext game)
    {
        // 斜率
        var k = Utility.Maths.LinerEquation.CountK(Owner.Transform.Position, Target);

        // 距离
        var d = Speed * game.GameTime.ElapsedTime;

        // 当前点
        var p1 = Owner.Transform.Position;

        var p2 = Utility.Maths.LinerEquation.GetP2(k, d, p1);

        return new Vector2(p2.X - Owner.Transform.Position.X, p2.Y - Owner.Transform.Position.Y) * Direction.X;
    }

}

