using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParticle : MonoBehaviour
{
    public Sprite sprite;

    public bool playAwake;
    public bool isLoop;

    [Range(0, 50)]
    public int maxParticle;

    [Range(0f, 100f)]
    public float speed;
    [Range(0f, 100f)]
    public float duration;
    [Range(0f, 100f)]
    public float lifeTime;

    [Range(0f, 100f)]
    public float startSize;
    [Range(0f, 100f)]
    public float targetSize;

    public Color startColor;
    public Color targetColor;

    private List<GameObject> particles;
    private IEnumerator particleRoutine;

    private int childIndex = 0;

    private void Awake()
    {
        particles = new List<GameObject>();

        for (int i = 0; i < maxParticle; i++)
        {
            GameObject particle = new GameObject("Particle");
            particle.transform.parent = transform;
            particle.transform.position = transform.position;

            Image image = particle.AddComponent<Image>();
            image.sprite = sprite;
            image.color = startColor;
            image.SetNativeSize();

            particle.SetActive(false);

            particles.Add(particle);
        }

        particleRoutine = CreateParticle();

        if(playAwake)
        {
            StartCoroutine(particleRoutine);
        }
    }

    private void OnDestroy()
    {
        Stop();
        particleRoutine = null;
    }

    public void Play()
    {
        StartCoroutine(particleRoutine);
    }

    public void Stop()
    {
        StopCoroutine(particleRoutine);
    }

    private IEnumerator CreateParticle()
    {
        float time = 0;

        while(true)
        {
            time += Time.deltaTime;

            if (time > duration)
            {
                StartCoroutine(RunParticle(particles[childIndex].transform, lifeTime));

                childIndex = (childIndex + 1) % maxParticle;

                time -= duration;
            }

            yield return null;
        }
    }


    private IEnumerator RunParticle(Transform particle, float time)
    {
        float startSize = this.startSize;
        float currentTime = 0f;
        float degree = Random.Range(0f, 359f);
        Image image = particle.GetComponent<Image>();

        particle.transform.localScale = Vector3.one * startSize;
        particle.transform.position = transform.position;
        particle.transform.rotation = Quaternion.Euler(0, 0, degree);
        particle.gameObject.SetActive(true);

        while (currentTime < time)
        {
            float t = currentTime / time;
            float lerp = Mathf.Lerp(startSize, targetSize, t);

            particle.localScale = Vector3.one * lerp;
            image.color = Color.Lerp(startColor, targetColor, t);

            currentTime += Time.deltaTime;

            yield return null;
        }

        particle.gameObject.SetActive(false);
    }
}
