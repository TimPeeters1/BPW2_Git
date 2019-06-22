using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource source;

    [SerializeField] AudioClip IntroClip;
    [SerializeField] AudioClip GameplayClip;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(NextMusic());
    }

    IEnumerator NextMusic()
    {
        source.clip = IntroClip;
        source.loop = false;
        source.volume = 1;
        source.Play();

        yield return new WaitForSeconds(IntroClip.length);

       
        source.clip = GameplayClip;
        source.loop = true;
        source.volume = 0.3f;
        source.Play();
    }
}
