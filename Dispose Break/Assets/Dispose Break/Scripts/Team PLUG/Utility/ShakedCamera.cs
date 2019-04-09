using System.Collections;
using UnityEngine;

namespace com.TeamPlug.Utility
{
    public class ShakedCamera : MonoBehaviour
    {
        public float amount = 0f;
        public float duration = 0f;

        private Vector3 originPosition;
        private bool isShaking = false;

        private void Start()
        {
            originPosition = transform.localPosition;
        }

        public void StartShake()
        {
            StartCoroutine(Shake());
        }

        public IEnumerator Shake()
        {
            if (!isShaking)
            {
                isShaking = true;

                float timer = 0f;

                while (timer <= duration)
                {
                    transform.localPosition = (Vector3)Random.insideUnitCircle * amount + originPosition;

                    timer += Time.deltaTime;
                    yield return null;
                }

                transform.localPosition = originPosition;

                isShaking = false;
            }
        }
    }
}