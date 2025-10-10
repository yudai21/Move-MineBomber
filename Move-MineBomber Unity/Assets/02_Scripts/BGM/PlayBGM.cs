using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGMType.Play);
    }

    void Update()
    {

    }

}
