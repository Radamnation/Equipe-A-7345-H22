using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioClip hubTheme;
    [SerializeField] private AudioClip explorationThemeOutOfCombat;
    [SerializeField] private AudioClip explorationThemeInCombat;

    private AudioSource myAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.loop = true;
        PlayHubTheme();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMusic()
    {
        myAudioSource.Stop();
    }

    public void PlayHubTheme()
    {
        myAudioSource.clip = hubTheme;
        myAudioSource.Play();
    }

    public void SwitchToInCombat()
    {
        var temp = myAudioSource.time;
        myAudioSource.clip = explorationThemeInCombat;
        myAudioSource.time = temp;
        myAudioSource.Play();
    }

    public void SwitchToOutOfCombat()
    {
        var temp = myAudioSource.time;
        myAudioSource.clip = explorationThemeOutOfCombat;
        myAudioSource.time = temp;
        myAudioSource.Play();
    }
}
