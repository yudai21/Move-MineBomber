using UnityEngine;

public class GameEndSE : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ButtonClick()
    {
        SoundManager.instance.PlaySE(SoundManager.SEType.Click);
    }

}
