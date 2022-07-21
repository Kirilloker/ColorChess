using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_script : MonoBehaviour
{
    public AudioSource _step;
    public AudioSource _dark_capture;
    public AudioSource _swith_lamp;
    public AudioSource _reverse_step;
    public AudioSource _dark_capture_reverse;

    public float _volume = 1;
    public bool _mute = false;

    bool costil_cansel = false;

    public void void_sound() 
    {
        costil_cansel = false;
    }
    public void step() 
    {
        play_audio(_step);
    }

    public void reverse_step()
    {
        costil_cansel = true;
        play_audio(_reverse_step);
    }

    public void dark_capture()
    {
        if (costil_cansel == false) play_audio(_dark_capture);
        //else play_audio(_dark_capture_reverse);
    }

    public void swith_lamp()
    {
        play_audio(_swith_lamp);
    }

    public void play_audio(AudioSource audio) 
    {
        audio.volume = _volume;
        audio.mute = _mute;

        audio.Play();
    }

}
