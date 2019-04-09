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

        // ----------- InputObservable Interface ---------------
        public virtual void TouchBegan(Vector3 _touchPosition, int _index)    {}
        public virtual void TouchMoved(Vector3 _touchPosition, int _index)    {}
        public virtual void TouchCancel(Vector3 _touchPosition, int _index)   {}
        public virtual void TouchEnded(Vector3 _touchPosition, int _index)    {}
    }
}
