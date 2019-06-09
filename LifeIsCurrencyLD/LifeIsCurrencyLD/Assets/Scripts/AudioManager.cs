using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    public Sound[] sounds;

    float timeSinceLastFart = 10f;
    bool fartEnded = true;

    private void Update()
    {
        timeSinceLastFart += Time.deltaTime;
        if (!fartEnded && timeSinceLastFart >= 10)
        {
            fartEnded = true;
            StartCoroutine(FadeSounds("BackgroundPostFart", "BackgroundPreFart"));
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach(var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("BackgroundPreFart");
    }

    public void Play(string name)
    {
        IsFartSound(name);
        var s = Array.Find(sounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
    }

    public void Play(string name, Vector3 position)
    {
        IsFartSound(name);
        var s = Array.Find(sounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    public void Stop(string name)
    {
        var s = Array.Find(sounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }

        s.source.Stop();
    }

    void IsFartSound(string name)
    {
        if (name.ToLower().Contains("fart") && !name.ToLower().Contains("background") && fartEnded)
        {
            StartCoroutine(DelayedBackgroundChange());
        }
    }

    IEnumerator DelayedBackgroundChange()
    {
        Stop("BackgroundPreFart");
        fartEnded = false;
        timeSinceLastFart = 0;
        yield return new WaitForSeconds(2);        
        Play("BackgroundPostFart");
        
    }

    IEnumerator FadeSounds(string name1, string name2)
    {
        var s1 = Array.Find(sounds, x => x.name == name1);
        var s2 = Array.Find(sounds, x => x.name == name2);
        var s1v = s1.volume;
        var s2v = s2.volume;

        var s1o = s1.source.volume;
        var s2o = s2.source.volume;
        s2.source.volume = 0;
        s2.source.Play();

        var steps = 15;
        for (int i = 1; i < steps; i++)
        {
            s1.source.volume = s1v / i;
            s2.source.volume = s2v / (steps - i);
            yield return new WaitForSeconds(0.1f);
        }
        s1.source.Stop();
        s1.source.volume = s1v;
        s2.source.volume = s2v;
        

    }
}
