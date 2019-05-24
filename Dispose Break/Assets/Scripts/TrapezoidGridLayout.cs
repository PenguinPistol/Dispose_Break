using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapezoidGridLayout : LayoutGroup
{
    public enum StartCorner
    {
        UpperLeft, UpperRight,
        LowerLeft, LowerRight
    }

    public enum Direction
    {
        Left, Right
    }

    public StartCorner startCorner;
    public Direction startDirection;
    public int columCount = 1;
    public Vector2 cellSize;
    public Vector2 spacing;
    public float concave = 0f;

    public override void CalculateLayoutInputVertical()
    {
        if (columCount < 1)
        {
            columCount = 1;
        }

        float rectWidth = rectTransform.rect.width;
        float rectHeight = rectTransform.rect.height;

        float startX = 0;
        float startY = 0;

        switch(childAlignment)
        {
            case TextAnchor.UpperLeft:
                startX = -rectWidth / 2f;
                break;
            case TextAnchor.UpperCenter:
                startX = -((cellSize.x * columCount) + (spacing.x * columCount) + concave) / 2f;
                break;
            case TextAnchor.UpperRight:
                startX = (rectWidth / 2f) - (cellSize.x * columCount) - (spacing.x * columCount) - concave;
                break;
            case TextAnchor.LowerLeft:
                startX = -rectWidth / 2f;
                break;
            case TextAnchor.LowerCenter:
                startX = -((cellSize.x * columCount) + (spacing.x * columCount) + concave) / 2f;
                break;
            case TextAnchor.LowerRight:
                startX = (rectWidth / 2f) - (cellSize.x * columCount) - (spacing.x * columCount) - concave;
                break;
        }

        for (int i = 0; i < rectChildren.Count; i++)
        {
            int xIndex = (i % columCount);
            int yIndex = (i / columCount);

            float x = startX + xIndex * cellSize.x + spacing.x * xIndex;
            float y = startY + yIndex * cellSize.y - rectHeight / 2 + spacing.y * yIndex;

            if ((i / columCount) % 2 == 1)
            {
                if (startDirection == Direction.Left)
                {
                    x -= concave;
                }
                else
                {
                    x += concave;
                }
            }

            rectChildren[i].anchorMin = Vector2.up;
            rectChildren[i].anchorMax = Vector2.up;
            rectChildren[i].pivot = Vector2.up;
            rectChildren[i].localPosition = new Vector3(x, -y);
        }
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}
