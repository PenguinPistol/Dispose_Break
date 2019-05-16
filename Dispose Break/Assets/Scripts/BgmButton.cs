using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmButton : MonoBehaviour
{
    public Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = !SoundManager.Instance.muteBgm;
        toggle.onValueChanged.AddListener(delegate {
            SoundManager.Instance.MuteBgm();
        });
    }

    void Update()
    {
        toggle.isOn = !SoundManager.Instance.muteBgm;
    }
}
