using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

namespace com.TeamPlug.Input
{
    public class TouchController : Singleton<TouchController>
    {
        /// <summary>
        /// 인식 할 터치 개수
        /// </summary>
        public static int MAX_TOUCH_COUNT = 1;

        private List<ITouchObservable> observableList;
        public int observableCount { get { return observableList.Count; } }

        private void Awake()
        {
            observableList = new List<ITouchObservable>();
        }

        private void Update()
        {
            if (UnityEngine.Input.touchSupported)
            {
                int touchCount = UnityEngine.Input.touchCount > MAX_TOUCH_COUNT ? MAX_TOUCH_COUNT : UnityEngine.Input.touchCount;

                for (int i = 0; i < touchCount; i++)
                {
                    Touch touch = UnityEngine.Input.GetTouch(i);
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            for (int j = 0; j < observableList.Count; j++)
                            {
                                observableList[j].TouchBegan(touchPosition, touch.fingerId);
                            }
                            break;
                        case TouchPhase.Stationary:
                        case TouchPhase.Moved:
                            for (int j = 0; j < observableList.Count; j++)
                            {
                                observableList[j].TouchMoved(touchPosition, touch.fingerId);
                            }
                            break;
                        case TouchPhase.Canceled:
                        case TouchPhase.Ended:
                            for (int j = 0; j < observableList.Count; j++)
                            {
                                observableList[j].TouchEnded(touchPosition, touch.fingerId);
                            }
                            break;
                    }
                }
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < observableList.Count; i++)
                    {
                        observableList[i].TouchBegan(mousePosition, 0);
                    }
                }
                else if (UnityEngine.Input.GetMouseButton(0))
                {
                    for (int i = 0; i < observableList.Count; i++)
                    {
                        observableList[i].TouchMoved(mousePosition, 0);
                    }
                }
                else if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    for (int i = 0; i < observableList.Count; i++)
                    {
                        observableList[i].TouchEnded(mousePosition, 0);
                    }
                }
                if (UnityEngine.Input.touchCount <= 0)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 터치 이벤트 오브젝트를 등록합니다.
        /// </summary>
        /// <param name="_observable"></param>
        public void AddObservable(ITouchObservable _observable)
        {
            if (observableList.Contains(_observable) || observableList == null)
            {
                return;
            }

            observableList.Add(_observable);
        }

        /// <summary>
        /// 등록된 터치 이벤트 오브젝트를 제거합니다.
        /// </summary>
        /// <param name="_observable"></param>
        public void RemoveObservable(ITouchObservable _observable)
        {
            if(observableList == null)
            {
                return;
            }

            observableList.Remove(_observable);
        }

        /// <summary>
        /// 포함한 레이어와 레이캐스팅을 실행합니다.
        /// </summary>
        /// <param name="touchPosition">터치 좌표</param>
        /// <param name="layers">레이캐스팅에 포함할 레이어</param>
        /// <returns></returns>
        public static Collider2D Raycast2D(Vector2 touchPosition, params int[] layers)
        {
            int layerMask = 1;

            for (int i = 0; i < layers.Length; i++)
            {
                layerMask = layerMask << layers[i];
            }

            return Physics2D.Raycast(touchPosition, Vector2.zero, 0f, ~layerMask).collider;
        }
    }

    public interface ITouchObservable
    {
        void TouchBegan(Vector3 touchPosition, int touchIndex);
        void TouchMoved(Vector3 touchPosition, int touchIndex);
        void TouchCancel(Vector3 touchPosition, int touchIndex);
        void TouchEnded(Vector3 touchPosition, int touchIndex);
    }
}