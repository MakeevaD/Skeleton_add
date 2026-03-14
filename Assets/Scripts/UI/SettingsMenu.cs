using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings UI")]
    public Slider volumeSlider;
    public Button soundToggleButton;
    public Image soundToggleImage;
    public TextMeshProUGUI soundToggleText;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public string soundOnText = "Звук вкл";
    public string soundOffText = "Звук выкл";
    
    [Header("Audio")]
    public AudioMixer audioMixer;
    public string volumeParameter = "MasterVolume";
    
    private bool isMuted = false;
    private float lastVolume = 0.69f;
    
    void Awake()
    {
        LoadSettings();
    }
    
    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;
            
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            
            volumeSlider.value = lastVolume;
            SetVolume(lastVolume);
        }
        
        if (soundToggleButton != null)
        {
            soundToggleButton.onClick.RemoveAllListeners();
            soundToggleButton.onClick.AddListener(ToggleSound);
        }
        
        UpdateSoundToggleUI();
    }
    
    public void OnVolumeChanged(float value)
    {
        lastVolume = value;
        SetVolume(value);
        SaveSettings();
    }
    
    void ToggleSound()
    {
        isMuted = !isMuted;
        
        if (isMuted)
        {
            SetVolume(0f);
        }
        else
        {
            SetVolume(lastVolume);
        }
        
        UpdateSoundToggleUI();
        SaveSettings();
    }
    
    void SetVolume(float volume)
    {
        float normalizedVolume = volume;
        
        if (audioMixer != null)
        {
            if (normalizedVolume <= 0.0001f)
                audioMixer.SetFloat(volumeParameter, -80f);
            else
            {
                float dB = Mathf.Log10(normalizedVolume) * 20;
                dB = Mathf.Clamp(dB, -80f, 0f);
                audioMixer.SetFloat(volumeParameter, dB);
            }
        }
        else if (AudioManager.instance != null)
        {
            var soundsField = typeof(AudioManager).GetField("sounds", 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
            if (soundsField != null)
            {
                var sounds = soundsField.GetValue(AudioManager.instance) as System.Array;
                
                foreach (var s in sounds)
                {
                    var sourceField = s.GetType().GetField("source");
                    if (sourceField != null)
                    {
                        var source = sourceField.GetValue(s) as AudioSource;
                        if (source != null)
                        {
                            source.volume = normalizedVolume;
                        }
                    }
                }
            }
        }
    }
    
    void UpdateSoundToggleUI()
    {
        if (soundToggleImage != null)
            soundToggleImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
            
        if (soundToggleText != null)
            soundToggleText.text = isMuted ? soundOffText : soundOnText;
    }
    
    void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundVolume", lastVolume);
        PlayerPrefs.SetInt("SoundMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    void LoadSettings()
    {
        lastVolume = PlayerPrefs.GetFloat("SoundVolume", 0.69f);
        isMuted = PlayerPrefs.GetInt("SoundMuted", 0) == 1;
    }
}
