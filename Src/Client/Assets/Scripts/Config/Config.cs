using UnityEngine;

//配置类
class Config
{
    //背景音乐
    public static bool MusicOn
    {
        get
        {
            return PlayerPrefs.GetInt("Music", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Music", value ? 1 : 0);
            SoundManager.Instance.MusicOn = value;
        }
    }
    //声音音效
    public static bool SoundOn
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
            SoundManager.Instance.SoundOn = value;
        }
    }

    //背景音乐音量
    public static int MusicVolume
    {
        get
        {
            return PlayerPrefs.GetInt("MusicVolume", 100);
        }
        set
        {
            PlayerPrefs.SetInt("MusicVolume", value);
            SoundManager.Instance.MusicVolume = value;
        }
    }
    //声音音效音量
    public static int SoundVolume
    {
        get
        {
            return PlayerPrefs.GetInt("SoundVolume", 100);
        }
        set
        {
            PlayerPrefs.SetInt("SoundVolume", value);
            SoundManager.Instance.SoundVolume = value;
        }
    }

    ~Config()
    {
        PlayerPrefs.Save();
    }
} 
