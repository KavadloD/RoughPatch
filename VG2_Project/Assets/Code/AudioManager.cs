using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip level;
    public AudioClip boss;
    private AudioSource _audio;

    public bool inBossRoom;
    
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = level;

        _audio.Play();

        inBossRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inBossRoom)
        {
            StartCoroutine(FadeOut(1f));

            inBossRoom = false;
        }
    }
    
    public IEnumerator FadeOut(float fadeTime) 
    {
        float startVolume = _audio.volume;
        while (_audio.volume > 0) {
            _audio.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        _audio.Stop();
        _audio.clip = boss;

        _audio.volume = 1;
        _audio.Play();
    }
}
