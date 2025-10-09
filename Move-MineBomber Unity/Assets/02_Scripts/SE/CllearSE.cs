using System.Diagnostics;
using UnityEngine;

public class CllearSE : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OnButtonClick()
    {
        SoundManager.instance.PlaySE(SoundManager.SEType.Click);
    }
}
