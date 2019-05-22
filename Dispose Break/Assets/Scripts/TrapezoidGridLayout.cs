using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapezoidGridLayout : GridLayoutGroup
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

    //public StartCorner startCorner;
    public Direction startDirection;
    public int columCount = 1;
    //public Vector2 cellSize;
    //public Vector2 spacing;
    public float concave = 0f;

    public override void CalculateLayoutInputVertical()
    {
        //if(columCount < 1)
        //{
        //    columCount = 1;
        //}

        //float rectWidth = rectTransform.rect.width;
        //float rectHeight = rectTransform.rect.height;

        //float startX = 0;
        //float startY = 0;

        //switch(childAlignment)
        //{
        //    case TextAnchor.UpperLeft:
        //        startX = -(rectWidth / 2f);
        //        startY = -(rectHeight / 2f);
        //        break;
        //    case TextAnchor.UpperCenter:
        //        startX = -(columCount / 2) * rectChildren[0].rect.width;
        //        startY = -(rectHeight / 2f);
        //        break;
        //    case TextAnchor.UpperRight:
        //        startY = -(rectHeight / 2f);
        //        break;
        //    case TextAnchor.MiddleLeft:
        //        break;
        //    case TextAnchor.MiddleCenter:
        //        break;
        //    case TextAnchor.MiddleRight:
        //        break;
        //    case TextAnchor.LowerLeft:
        //        break;
        //    case TextAnchor.LowerCenter:
        //        break;
        //    case TextAnchor.LowerRight:
        //        break;
        //}

        //for (int i = 0; i < rectChildren.Count; i++)
        //{
        //    int xIndex = (i % columCount);
        //    int yIndex = (i / columCount);

        //    float width = rectChildren[i].sizeDelta.x;
        //    float height = rectChildren[i].sizeDelta.y;
        //    float x = xIndex * width + spacing.x * xIndex;
        //    float y = yIndex * height - rectHeight / 2 + spacing.y * yIndex;

        //    switch(startCorner)
        //    {
        //        case StartCorner.UpperLeft:
        //            x -= rectWidth / 2;
        //            break;
        //        //case StartCorner.UpperCenter:
        //        //    x -= ((width * columCount) / 2);
        //        //    break;
        //        case StartCorner.UpperRight:
        //            x += (rectWidth / 2) - (width * columCount);
        //            break;

        //        case StartCorner.LowerLeft:
        //            x -= rectWidth / 2;
        //            break;

        //        case StartCorner.LowerRight:
        //            x -= rectWidth / 2;
        //            break;
        //    }

        //    if ((i / columCount) % 2 == 1)
        //    {
        //        if (startDirection == Direction.Left)
        //        {
        //            x -= concave;
        //        }
        //        else
        //        {
        //            x += concave;
        //        }
        //    }

        //    rectChildren[i].anchorMin = Vector2.up;
        //    rectChildren[i].anchorMax = Vector2.up;
        //    rectChildren[i].pivot = Vector2.up;
        //    rectChildren[i].localPosition = new Vector3(x, -y);
        //}
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}
