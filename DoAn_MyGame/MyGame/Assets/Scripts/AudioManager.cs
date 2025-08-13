using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backGroundAudioSoucre;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backGroundClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip coinClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackGroundMusic();
        PlayCoinSound();
        PlayJumpSound();
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

    public void PlayJumpSound()
    {
        effectAudioSource.PlayOneShot(jumpClip);
    }
    public void PlayCoinSound()
    {
        effectAudioSource.PlayOneShot(coinClip);
    }
}
