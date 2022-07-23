using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource soundStep;
    [SerializeField]
    private AudioSource soundDarkCapture;
    [SerializeField]
    private AudioSource soundReverseStep;
    [SerializeField]
    private AudioSource soundReverseDarkCapture;


    private float _volume = 1;
    private bool _mute = false;

    AudioSource sound;

    public void PlayAudio(SoundType audioType)
    {
        switch (audioType)
        {
            case SoundType.Step:
                sound = soundStep;
                break;
            case SoundType.DarkCapture:
                sound = soundDarkCapture;
                break;
            case SoundType.ReverseStep:
                sound = soundReverseStep;
                break;
            case SoundType.ReverseDarkCapture:
                sound = soundReverseDarkCapture;
                break;
            default:
                break;
        }

        sound.volume = _volume;
        sound.mute = _mute;

        sound.Play();
    }

}
