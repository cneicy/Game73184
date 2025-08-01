using Event;
using GamePlay.Objects.Heroes;
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
        //20是预留数，之后可能会改成别的变量
        if (finalDamage > 20 * 0.8f)
        {
            heroHitAudioSource[0].Play();
        }
        else if (finalDamage > 20 * 0.6f)
        {
            heroHitAudioSource[1].Play();
        }
        else if (finalDamage > 20 * 0.4f)
        {
            heroHitAudioSource[2].Play();
        }
        else if (finalDamage > 20 * 0.2f)
        {
            heroHitAudioSource[3].Play();
        }
        else heroHitAudioSource[4].Play();

        return null;
    }
}