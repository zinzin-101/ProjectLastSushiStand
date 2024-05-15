using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public static class SoundManager
{

    public enum Sound
    {
        Walk,
        AutoRifle,
        DoubleJump,
        WallJump,
        Jump,
        Melee,
        BGM,
        ReloadAssult,
        EnemyHitted,
        EnemyDead,
        bgm2,
        bgm3,
        EnemyShoot,
        Slide,
        GroundImpact,
        Run,
        crouch,
        Pistol,
    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.Walk] = 0f;
    }
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("GeneratedSound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            if (sound == Sound.BGM || sound == Sound.bgm2 || sound == Sound.bgm3)
            {
                audioSource.loop = true;
            }
            else
            {
                DestroySound destroySound = soundGameObject.AddComponent<DestroySound>();
                destroySound.delay = 180f;
            }

            audioSource.PlayOneShot(GetAudioClip(sound));
        }

    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.Walk:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .3f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            case Sound.Run:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .1f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            case Sound.crouch:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .6f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }

        }

    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioclip in GameAssets.i.soundAudioClipArray)
        {
            if (soundAudioclip.sound == sound)
            {
                return soundAudioclip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;
    }

}
