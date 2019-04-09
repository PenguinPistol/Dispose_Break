using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace com.TeamPlug.Patterns
{
    public class StateController : Singleton<StateController>
    {
        public List<State> stateList;
        public LoadingView loadingView;

        private State currentState;
        private Dictionary<string, State> states;

        public State CurrentState { get { return currentState; } }
        public string CurrentStateName { get { return currentState.name; } }

        public void Init()
        {
            states = new Dictionary<string, State>();

            for (int i = 0; i < stateList.Count; i++)
            {
                states.Add(stateList[i].GetType().Name, stateList[i]);
            }
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.Execute();
            }
        }

        public void ChangeState(string _name, bool _animated, params object[] _data)
        {
            StartCoroutine(Change(_name, _animated, _data));
        }

        private IEnumerator Change(string _name, bool _animated, params object[] _data)
        {
            if (loadingView != null && _animated)
            {
                yield return loadingView.StartAnimation();
            }

            var beforeState = currentState;

            if (currentState != null)
            {
                currentState.Release();
                Destroy(currentState.gameObject);
            }

            if (states.ContainsKey(_name))
            {
                currentState = Instantiate(states[_name]);
            }
            else
            {
                Debug.Log("state key is not contain");
                currentState = beforeState;
            }

            yield return currentState.Initialize(_data);

            if (loadingView != null && _animated)
            {
                //loadingView.SetActive(false);
                yield return loadingView.FinishAnimation();
            }

            currentState.Begin();
        }
    }
}