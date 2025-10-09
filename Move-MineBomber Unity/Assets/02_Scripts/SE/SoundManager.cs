using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //BGM
    public enum BGMType
    {
        Title=0,
        Play=1
    }

    //SE
    public enum SEType
    {
        Click=0,
        Bomb=1,
        GameOver=2,
        Caveat=3,
        GameClear=4
    }

    //クロスフェード時間
    public const float CROSS_FADE_TIME = 1f;

    public AudioClip[] BGMClips;
    public AudioClip[] SEClips;

    private AudioSource[] BGM_Sources = new AudioSource[2];
    private AudioSource[] SE_Sources = new AudioSource[8];

    private bool isCrossFading;

    private int currentBgmIndex = 999;

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //BGM用AudioSource追加
        BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
        BGM_Sources[1] = gameObject.AddComponent<AudioSource>();


        // SE用 AudioSource追加
        for (int i = 0; i < SE_Sources.Length; i++)
        {
            SE_Sources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }


    //BGM再生
    public void PlayBGM(BGMType bgmtype,bool loopFlg=true)
    {
        int index = (int)bgmtype;
        currentBgmIndex = index;

        if (BGM_Sources[0].clip != null && BGM_Sources[0].clip == BGMClips[index])
        {
            return;
        }
        else
        {
            if (BGM_Sources[1].clip != null && BGM_Sources[1].clip == BGMClips[index])
            {
                return;
            }
        }
        //フェードでBGM開始
        if (BGM_Sources[0].clip == null && BGM_Sources[1].clip==null)
        {
            BGM_Sources[0].loop = loopFlg;
            BGM_Sources[0].clip = BGMClips[index];
            BGM_Sources[0].Play();
        }
        else
        {//クロスフェード処理
            StartCoroutine(CrossFadeChangeBGM(index, loopFlg));
        }
    }

    //BGMのクロスフェード処理
    private IEnumerator CrossFadeChangeBGM(int index,bool loopFlg)
    {
        isCrossFading = true;
        if (BGM_Sources[0].clip!=null)
        {
            BGM_Sources[1].clip = BGMClips[index];
            BGM_Sources[1].loop = loopFlg;
            BGM_Sources[1].Play();
            BGM_Sources[1].DOFade(1.0f, CROSS_FADE_TIME).SetEase(Ease.Linear);
            BGM_Sources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGM_Sources[0].Stop();
            BGM_Sources[0].clip = null;
        }
        else
        {
            BGM_Sources[0].clip = BGMClips[index];
            BGM_Sources[0].loop = loopFlg;
            BGM_Sources[0].Play();
            BGM_Sources[0].DOFade(1.0f, CROSS_FADE_TIME).SetEase(Ease.Linear);
            BGM_Sources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGM_Sources[1].Stop();
            BGM_Sources[1].clip = null;
        }
        isCrossFading = false;
    }

    //BGM完全停止
    public void StopBGM()
    {
        BGM_Sources[0].Stop();
        BGM_Sources[1].Stop();
        BGM_Sources[0].clip = null;
        BGM_Sources[1].clip = null;
    }

    //SE再生
    public void PlaySE(SEType seType)
    {
        int index = (int)seType;
        if(index<0||SEClips.Length<=index)
        {
            return;
        }

        //再生中ではないAudioSourceを使ってSEをならす
        foreach (AudioSource source in SE_Sources)
        {
            // 再生中の AudioSource の場合には次のループ処理へ移る
            if (source.isPlaying)
            {
                continue;
            }

            // 再生中でない AudioSource に Clip をセットして SE を鳴らす
            source.clip = SEClips[index];
            source.Play();
            break;
        }    
    }
    //SE停止
    public void StopSE()
    {
        foreach(AudioSource source in SE_Sources)
        {
            source.Stop();
            source.clip = null;
        }
    }
}
