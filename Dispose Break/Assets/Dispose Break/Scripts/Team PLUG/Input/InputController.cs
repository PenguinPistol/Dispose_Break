using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

namespace com.TeamPlug.Input
{
    public class InputController : Singleton<InputController>
    {
        public const int MAX_TOUCH_COUNT = 2;

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

        public void AddObservable(ITouchObservable _observable)
        {
            if (observableList.Contains(_observable))
            {
                return;
            }

            observableList.Add(_observable);
        }

        public void RemoveObservable(ITouchObservable _observable)
        {
            observableList.Remove(_observable);
        }
    }
}