using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystemConfig : UIWindow
{
    public Image musicOff;
    public Image soundOff;

    public Toggle toggleMusic;
    public Toggle toggleSound;

    public Slider sliderMusic;
    public Slider sliderSound;

    private void Start()
    {
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
    }

    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }

    public void MusicToggle() 
    {
        musicOff.enabled = !toggleMusic.isOn;
        Config.MusicOn = toggleMusic.isOn;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void SoundToggle()
    {
        soundOff.enabled = !toggleSound.isOn;
        Config.SoundOn = toggleSound.isOn;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void MusicVolume() 
    {
        Config.MusicVolume = (int)(sliderMusic.value*100);
        PlaySound();
    }

    public void SoundVolume()
    {
        Config.SoundVolume = (int)(sliderSound.value*100);
        PlaySound();
    }

    float lastPlay = 0;
    private void PlaySound() 
    {
        if (Time.realtimeSinceStartup - lastPlay > 0.1) 
        {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
}
