using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class CharacterSounds : MonoBehaviour
{
    [EventRef]
    public string footstepSound = "event:/footstep_generic";
    [EventRef]
    public string walkSound = "event:/walk_cloth";
    [EventRef]
    public string jumpSound = "event:/jump_cloth";
    [EventRef]
    public string swingSound = "event:/swing";

    float nextSoundTime = 0;
    float footstepWeight = 1f;
    public float soundCooldown = 0.2f;

    //play custom sound from string, include pitch in string, e.g. 'hit_rock 1'
    public void PlaySound(string soundName)
    {
        string[] parsedStrings = soundName.Split(new string[] { " " }, System.StringSplitOptions.None);

        float pitch = 1f;
        soundName = "event:/" + parsedStrings[0];

        if(parsedStrings.Length > 0)
            pitch = float.Parse(parsedStrings[1]);

        FMOD.Studio.EventInstance soundEvent = RuntimeManager.CreateInstance(soundName);
        soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        soundEvent.setPitch(pitch);
        soundEvent.start();
        soundEvent.release();
    }

    public void PlayFootstepSound()
    {
        if (Time.time > nextSoundTime)
        {
            nextSoundTime = Time.time + soundCooldown;
            FMOD.Studio.EventInstance soundEvent = RuntimeManager.CreateInstance(footstepSound);
            FMOD.Studio.ParameterInstance param;
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            soundEvent.getParameter("Velocity", out param);
            param.setValue(footstepWeight);
            soundEvent.start();
            soundEvent.release();
        }
    }

    //cloth/armor sounds, etc.
    public void PlayWalkSound(float pitch = 1)
    {
        if (pitch == 0) pitch = 1;
        EventInstance soundEvent = RuntimeManager.CreateInstance(walkSound);
        soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        soundEvent.setPitch(pitch);
        soundEvent.start();
        soundEvent.release();
    }

    public void PlayJumpSound()
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(jumpSound);
        soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        soundEvent.start();
        soundEvent.release();
    }

    //swing melee weapon, pitch down for heavier weapons
    public void PlaySwingSound(float pitch = 1)
    {
        if (pitch == 0) pitch = 1;
        EventInstance soundEvent = RuntimeManager.CreateInstance(swingSound);
        soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        soundEvent.setPitch(pitch);
        soundEvent.start();
        soundEvent.release();
    }
}