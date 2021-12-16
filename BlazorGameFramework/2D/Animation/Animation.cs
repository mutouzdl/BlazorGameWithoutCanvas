using System.Drawing;
using System.Numerics;

using Microsoft.AspNetCore.Components;

namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public class Animation
{
    private readonly Transform _transform;

    private int _currFrameIndex = 0;
    private Vector2 _currFramePos = Vector2.Zero;
    /// <summary>
    /// 整图水平方向帧数
    /// </summary>
    private int _imageHorizontalFrameCount = 0;

    private int _playTimes = 0;
    private float _lastUpdate = 0f;

    public Animation(
        GameObject owner,
        string name,
        Size frameSize,
        Size imageSize,
        int startFrame,
        int framesCount,
        int fps,
        ElementReference imageRef
    )
    {
        _transform = owner.GetComponent<Transform>() ??
                     throw new Exception($"Transform not found on owner");

        Name = name;
        FrameSize = frameSize;
        ImageSize = imageSize;
        StartFrame = startFrame;
        ImageRef = imageRef;
        FramesCount = framesCount;
        FPS = fps;

        _imageHorizontalFrameCount = ImageSize.Width / FrameSize.Width;
    }

    public void Play()
    {
        Reset();

        UpdateCurrentFramePos();

        Playing = true;
    }

    public void Stop()
    {
        Playing = false;
    }

    public void Reset()
    {
        _currFrameIndex = 0;
        _lastUpdate = 0;
        _playTimes = 0;
    }

    public async ValueTask Render(GameContext game)
    {
        if (Playing == false)
        {
            return;
        }

        if (game.GameTime.TotalTime - _lastUpdate > 1f / FPS)
        {
            _lastUpdate = game.GameTime.TotalTime;

            UpdateCurrentFramePos();

            _currFrameIndex++;
        }

        await game.Canvas.SaveAsync();

        var dx = -(_transform.Direction.X - 1f) * FrameSize.Width / 2f;
        await game.Canvas.SetTransformAsync(_transform.Direction.X, 0, 0, 1, dx, 0);

        await game.Canvas.DrawImageAsync(ImageRef, _currFramePos.X, _currFramePos.Y,
            FrameSize.Width, FrameSize.Height,
            _transform.Position.X, _transform.Position.Y,
            FrameSize.Width, FrameSize.Height);

        await game.Canvas.RestoreAsync();

        _playTimes++;

        if (_currFrameIndex == FramesCount)
        {
            _playTimes++;

            if (Loop < 0 || _playTimes < Loop)
            {
                Reset();
            }
            else
            {
                Stop();
            }
        }
    }

    /// <summary>
    /// 更新当前帧的绘制坐标
    /// </summary>
    private void UpdateCurrentFramePos()
    {
        // x：初始计算：当前帧*帧宽度；  y：初始计算：当前帧*帧高度
        // 之后还要加上起点坐标（因为一张图里可能包含了多种动作动画，每种动画的起始位置不一样）
        // 加上起点后，x或y可能会发生变化，比如x的宽度超出了图片宽度，需要换行

        int xFrameIndex = _currFrameIndex + StartFrame;
        int yFrameIndex = xFrameIndex / _imageHorizontalFrameCount;

        if (yFrameIndex > 0 && xFrameIndex % _imageHorizontalFrameCount > 0)
        {
            yFrameIndex++;
        }

        _currFramePos.X = (xFrameIndex % _imageHorizontalFrameCount) * FrameSize.Width;
        _currFramePos.Y = yFrameIndex * FrameSize.Height;
    }

    public string Name { get; }
    public int FramesCount { get; }
    public int FPS { get; }
    /// <summary>
    /// 起点帧
    /// 一张图里可能包含了多种动作动画，每种动画的起始位置不一样，使用StartFrame标注起始位置在第几帧
    /// </summary>
    public int StartFrame { get; }
    public bool Playing { get; private set; } = false;
    /// <summary>
    /// 循环次数
    /// -1代表无限循环
    /// </summary>
    public int Loop { get; set; } = -1;
    public Size FrameSize { get; }
    public Size ImageSize { get; }
    public ElementReference ImageRef { get; set; }
}