using UnityEngine;

public struct OriginalAlignment
{
    public OriginalAlignment(float width = 1920f, float height = 1080f)
    {
        Width = width; Height = height;
    }
    public float Width { get; }
    public float Height { get; }

    public float HalfWidth { get { return Width / 2; } }
    public float HalfHeight { get { return Height / 2; } }

    public float QuarterWidth { get { return Width / 4; } }
    public float QuarterHeight { get { return Height / 4; } }
}

public class PopUpMessage: Beat
{
    private Vector2 originalPoint = new Vector2(0f, 700f);
    private Vector2 displayPoint = new Vector2(0f, 350f);
    public Vector2 OriginalPoint { get { return originalPoint; } set { originalPoint = value; } }
    public Vector2 DisplayPoint { get { return displayPoint; } set { displayPoint = value; } }
}
