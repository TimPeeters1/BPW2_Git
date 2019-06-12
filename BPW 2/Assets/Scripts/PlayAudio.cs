using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    AudioSource source;

    public AudioClip[] sounds;

    public void Awake()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();

        yield return new WaitForSeconds(source.clip.length);

        Destroy(this.gameObject);
    }
}
