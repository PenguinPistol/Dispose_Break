using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    public ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        while(ps.IsAlive(true))
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
