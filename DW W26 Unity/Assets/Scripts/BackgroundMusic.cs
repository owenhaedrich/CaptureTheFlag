using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    [SerializeField] AudioSource Source;

    [SerializeField] AudioClip MenuMusic;
    [SerializeField] AudioClip BattleMusic;
    [SerializeField] AudioClip EndGameMusic;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Play(AudioClip clip)
    {
        if (Source == null || clip == null) return;
        if (Source.clip == clip && Source.isPlaying) return;

        Source.Stop();
        Source.clip = clip;
        Source.loop = true;
        Source.Play();
    }

    public void PlayMenu() => Play(MenuMusic);
    public void PlayBattle() => Play(BattleMusic);
    public void PlayEnd() => Play(EndGameMusic);
}
