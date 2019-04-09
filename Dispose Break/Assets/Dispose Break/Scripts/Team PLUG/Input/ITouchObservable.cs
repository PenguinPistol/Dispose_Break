using UnityEngine;

namespace com.TeamPlug.Input
{
    public interface ITouchObservable
    {
        void TouchBegan(Vector3 _touchPosition, int _index);
        void TouchMoved(Vector3 _touchPosition, int _index);
        void TouchCancel(Vector3 _touchPosition, int _index);
        void TouchEnded(Vector3 _touchPosition, int _index);
    }
}
