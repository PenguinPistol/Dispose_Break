using UnityEngine;
using System.Collections;
using com.TeamPlug.Input;

namespace com.TeamPlug.Patterns
{
    public abstract class State : MonoBehaviour, ITouchObservable
    {
        public abstract IEnumerator Initialize(params object[] _data);
        public abstract void Begin();
        public abstract void Execute();
        public abstract void Release();

        // ITouchObservable Interface
        public virtual void TouchBegan(Vector3 touchPosition, int touchIndex)    {}
        public virtual void TouchMoved(Vector3 touchPosition, int touchIndex)    {}
        public virtual void TouchCancel(Vector3 touchPosition, int touchIndex)   {}
        public virtual void TouchEnded(Vector3 touchPosition, int touchIndex)    {}
    }
}
