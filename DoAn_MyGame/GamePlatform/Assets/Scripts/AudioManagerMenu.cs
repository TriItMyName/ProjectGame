using UnityEngine;

public class AudioManagerMenu : MonoBehaviour
{
    [SerializeField] private AudioSource backGroundAudioSoucre;

    [SerializeField] private AudioClip backGroundClip;

    void Start()
    {
        PlayBackGroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackGroundMusic()
    {
        backGroundAudioSoucre.clip = backGroundClip;
        backGroundAudioSoucre.Play();
    }
}
