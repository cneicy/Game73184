using DG.Tweening;
using Event;
using GamePlay;
using GamePlay.Objects.Heroes;
using Singleton;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource[] heroHitAudioSource;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource powerClick;
    [SerializeField] private AudioSource btnClick;
    private Hero _hero;
    
    [Header("BGM音高变化曲线")]
    [Tooltip("控制BGM音高随血量变化的曲线")]
    public AnimationCurve bgmPitchCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    [Header("BGM音高范围")]
    [Tooltip("血量满时的音高")]
    [Range(0.1f, 3f)]
    public float minPitch = 1f;
    [Tooltip("血量空时的音高")]
    [Range(0.1f, 3f)]
    public float maxPitch = 1.5f;
    
    [Header("音高变化过渡时间")]
    public float pitchTransitionDuration = 1f;
    
    private Tween _pitchTween;

    private void Start()
    {
        _hero = FindObjectOfType<Hero>();
        bgm.pitch = minPitch;
    }

    private void OnEnable()
    {
        if (EventManager.Instance)
            EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        bgm.loop = true;
    }

    private void OnDisable()
    {
        if (EventManager.Instance)
            EventManager.Instance.UnregisterAllEventsForObject(this);
        bgm.Stop();
        _pitchTween?.Kill();
    }

    [EventSubscribe("TurningNext")]
    public object OnTurningNext(GameState gameState)
    {
        btnClick.Play();
        return null;
    }
    [EventSubscribe("TurningPrevious")]
    public object OnTurningPrevious(GameState gameState)
    {
        btnClick.Play();
        return null;
    }

    [EventSubscribe("PowerButtonClick")]
    public object OnPowerButtonClick(string anyway)
    {
        powerClick.Play();
        return null;
    }

    [EventSubscribe("PowerOn")]
    public object OnGameStart(string anyway)
    {
        bgm.Play();
        return null;
    }

    [EventSubscribe("GameOver")]
    public object OnGameOver(string anyway)
    {
        bgm.Stop();
        return null;
    }

    [EventSubscribe("Harvest")]
    public object OnHarvest(int finalDamage)
    {
        var healthPercent = _hero.Health / (float)Hero.MaxHealth;
        var factor = 1 - healthPercent;
        
        var curveValue = bgmPitchCurve.Evaluate(factor);
        
        var targetPitch = Mathf.Lerp(minPitch, maxPitch, curveValue);
        
        _pitchTween?.Kill();
        
        _pitchTween = DOTween.To(() => bgm.pitch, 
                                x => bgm.pitch = x, 
                                targetPitch, pitchTransitionDuration);
        
        HandleHeroHitSound(finalDamage);
        
        return null;
    }

    private void HandleHeroHitSound(int finalDamage)
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
        else 
        {
            heroHitAudioSource[4].Play();
        }
    }
}