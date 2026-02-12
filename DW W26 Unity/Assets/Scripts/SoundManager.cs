using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource Source;

    [SerializeField] AudioClip RoundStart;
    [SerializeField] AudioClip FlagSnatched;
    [SerializeField] AudioClip FlagCaptured;
    [SerializeField] AudioClip DashKill;
    [SerializeField] AudioClip Teleporter;

    [SerializeField] AudioClip Dash;
    [SerializeField] AudioClip StepOn;
    [SerializeField] AudioClip StepOff;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Play(AudioClip clip)
    {
        if (Source == null || clip == null) return;
        Source.PlayOneShot(clip);
    }

    public void PlayRoundStart() => Play(RoundStart);
    public void PlayFlagSnatched() => Play(FlagSnatched);
    public void PlayFlagCaptured() => Play(FlagCaptured);
    public void PlayDashKill() => Play(DashKill);
    public void PlayTeleporter() => Play(Teleporter);

    public void PlayDash() => Play(Dash);
    public void PlayStepOn() => Play(StepOn);
    public void PlayStepOff() => Play(StepOff);
}
