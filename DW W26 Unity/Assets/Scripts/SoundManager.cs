using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource Source;

    [Header("Round / UI")]
    [SerializeField] AudioClip RoundStart;

    [Header("Flags")]
    [SerializeField] AudioClip FlagSnatched;
    [SerializeField] AudioClip FlagCaptured;
    [SerializeField] AudioClip FlagDropped;

    [Header("Player")]
    [SerializeField] AudioClip Dash;
    [SerializeField] AudioClip DashKill;
    [SerializeField] AudioClip Respawn;

    [Header("World")]
    [SerializeField] AudioClip Teleporter;
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
    public void PlayFlagDropped() => Play(FlagDropped);

    public void PlayDash() => Play(Dash);
    public void PlayDashKill() => Play(DashKill);
    public void PlayRespawn() => Play(Respawn);

    public void PlayTeleporter() => Play(Teleporter);
    public void PlayStepOn() => Play(StepOn);
    public void PlayStepOff() => Play(StepOff);
}