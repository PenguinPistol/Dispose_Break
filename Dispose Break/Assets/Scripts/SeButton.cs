using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeButton : MonoBehaviour
{
    public Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = !SoundManager.Instance.muteSe;
        toggle.onValueChanged.AddListener(delegate {
            SoundManager.Instance.MuteSe();
        });
    }

    void Update()
    {
        toggle.isOn = !SoundManager.Instance.muteSe;
    }
}
