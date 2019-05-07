using UnityEngine;
using System.Collections;

public class CameraScaler : MonoBehaviour
{
    public const float TARGET_WIDTH_RATIO = 9f;
    public const float TARGET_HEIGHT_RATIO = 16f;

    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();

        if(camera == null)
        {
            return;
        }

        float targetAspect = TARGET_WIDTH_RATIO / TARGET_HEIGHT_RATIO;
        float newAspect = targetAspect / camera.aspect;

        camera.orthographicSize = camera.orthographicSize * newAspect;

        #region 해상도 고정(위아래 빈공간 삽입)
        //camera.aspect = TARGET_WIDTH_RATIO / TARGET_HEIGHT_RATIO;

        //float widthRatio = Screen.width / TARGET_WIDTH_RATIO;
        //float heightRatio = Screen.height / TARGET_HEIGHT_RATIO;

        //float heightAdd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
        //float widthAdd = ((heightRatio / (widthRatio / 100)) - 100) / 200;

        //if (heightRatio > widthRatio)
        //{
        //    widthAdd = 0f;
        //}
        //else
        //{
        //    heightAdd = 0f;
        //}

        //camera.rect = new Rect(
        //    camera.rect.x + Mathf.Abs(widthAdd),
        //    camera.rect.y + Mathf.Abs(heightAdd),
        //    camera.rect.width + (widthAdd * 2),
        //    camera.rect.height + (heightAdd * 2));
        #endregion
    }
}
