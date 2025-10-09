using UnityEngine;

public class GameClearSE : MonoBehaviour
{
    [SerializeField] private AudioClip clickSE;
    [SerializeField] private AudioClip clearSE;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void playclickSE()
    {
        audioSource.PlayOneShot(clickSE);
    }

    public void playclearSE()
    {
        audioSource.PlayOneShot(clearSE);
    }
}
