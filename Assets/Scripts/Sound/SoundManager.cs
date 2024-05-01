using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public static class SoundManager
{

    public enum Sound
    {
        Blanked,
        FindingLockPick,
        PlayerMove,
        KeyPickUp,
        OpenDrawer,
        Paper,
        ChairDown,
        PlaceComClue,
        StandOnChair,
        UseKey,
        WardropeOpen,
        PickupItem,
        Dialog,
        Start,
        BGM

    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
    }
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            DestroySound destroySound = soundGameObject.AddComponent<DestroySound>();
            destroySound.delay = 20f;
            audioSource.PlayOneShot(GetAudioClip(sound));
        }

    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.PlayerMove:
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
                    return false;
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
        Debug.LogError("Sound" + sound + "not found");
        return null;
    }
}
