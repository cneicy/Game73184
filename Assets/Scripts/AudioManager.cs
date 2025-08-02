using DG.Tweening;
using Event;
using GamePlay.Objects.Heroes;
using Singleton;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource[] heroHitAudioSource;
    [SerializeField] private AudioSource bgm;
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
        // 初始化BGM音高
        bgm.pitch = minPitch;
    }

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
        // 停止所有正在进行的音高过渡
        _pitchTween?.Kill();
    }

    [EventSubscribe("Harvest")]
    public object OnHarvest(int finalDamage)
    {
        var healthPercent = _hero.Health / (float)Hero.MaxHealth;
        var factor = 1 - healthPercent; // 血量越低，factor越大（0到1）
        
        // 使用曲线计算音高变化强度
        var curveValue = bgmPitchCurve.Evaluate(factor);
        
        // 计算目标音高值
        var targetPitch = Mathf.Lerp(minPitch, maxPitch, curveValue);
        
        // 停止之前的音高过渡
        _pitchTween?.Kill();
        
        // 使用DOTween平滑过渡音高
        _pitchTween = DOTween.To(() => bgm.pitch, 
                                x => bgm.pitch = x, 
                                targetPitch, pitchTransitionDuration);
        
        // 处理英雄受击音效
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