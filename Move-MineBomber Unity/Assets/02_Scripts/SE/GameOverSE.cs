using UnityEngine;

public class GameOverSE : MonoBehaviour
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
