using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeStar : MonoBehaviour
{
    public ParticleSystem particle1;
    public ParticleSystem particle2;

    public void PlayParticle()
    {
        particle1.Play();
        particle2.Play();
    }

}
