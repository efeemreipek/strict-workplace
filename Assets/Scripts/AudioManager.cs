using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int poolSize = 10;

    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip crateClip;

    private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();

    protected override void Awake()
    {
        base.Awake();

        InitializePool();
    }

    private void InitializePool()
    {
        for(int i = 0;  i < poolSize; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab, transform);
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = obj.AddComponent<AudioSource>();
            }

            obj.SetActive(false);
            audioSourcePool.Enqueue(audioSource);
        }
    }
    private AudioSource GetPooledAudioSource()
    {
        if(audioSourcePool.Count > 0)
        {
            AudioSource audioSource = audioSourcePool.Dequeue();
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }
        else
        {
            GameObject obj = Instantiate(audioSourcePrefab, transform);
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = obj.AddComponent<AudioSource>();
            }
            return audioSource;
        }
    }
    private void ReturnToPool(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        audioSourcePool.Enqueue(source);
    }
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if(clip == null) return;

        AudioSource source = GetPooledAudioSource();
        source.volume = volume;
        source.PlayOneShot(clip);

        StartCoroutine(DisableAfterPlay(source, clip.length));
    }
    private System.Collections.IEnumerator DisableAfterPlay(AudioSource source, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        ReturnToPool(source);
    }
    public void PlayMoveSFX(float volume = 1f) => PlaySFX(moveClip, volume);
    public void PlayCrateSFX(float volume = 1f) => PlaySFX(crateClip, volume);
}
