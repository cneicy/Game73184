using Event;
using Singleton;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource[] heroHitAudioSource;
    [SerializeField] private AudioSource bgm;
    
    private void OnEnable()
    {
        if (EventManager.Instance)
            EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        bgm.Play();
        bgm.loop = true;
    }

    private void OnDisable()
    {
        if (EventManager.Instance)
            EventManager.Instance.UnregisterAllEventsForObject(this);
        bgm.Stop();
    }

    [EventSubscribe("Harvest")]
    public object OnHeroHit(float finalDamage)
    {
        switch (finalDamage)
        {
            case >= 0.8f:
                heroHitAudioSource[0].Play();
                break;
            case > 0.6f:
                heroHitAudioSource[1].Play();
                break;
            case > 0.4f:
                heroHitAudioSource[2].Play();
                break;
            case > 0.2f:
                heroHitAudioSource[3].Play();
                break;
            case > 0.0f:
                heroHitAudioSource[4].Play();
                break;
        }

        return null;
    }
}