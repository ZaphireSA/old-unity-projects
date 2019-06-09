using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;

	public static AudioManager instance;
	public AudioSource backgroundSource;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
		DontDestroyOnLoad(gameObject);
	}

    private void Start()
    {
        ToggleBackgroundVolume(StageManager.instance.playerData.BackgroundMusic);
    }

    public void ToggleBackgroundVolume(bool isOn)
    {
        backgroundSource.volume = isOn ? 1 : 0;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public void Play(string name) {
		Sound s = sounds.Where (x => x.name == name).First();        
        s.source.Play ();
	}

    public void Play(string name, float pitch)
    {
        Sound s = sounds.Where(x => x.name == name).First();
        s.source.pitch = pitch;
        s.source.Play();
    }

	public void Play(string name, Vector3 pos) {
		Sound s = sounds.Where (x => x.name == name).First();
		AudioSource.PlayClipAtPoint(s.clip, pos, s.volume);
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
        backgroundSource.Stop();
        if (scene.name == "Main Menu") {
			Sound s = sounds.Where (x => x.name == "mainmenu_background").First ();
			backgroundSource.clip = s.clip;
			backgroundSource.volume = StageManager.instance.playerData.BackgroundMusic ? s.volume : 0;
			backgroundSource.pitch = s.pitch;
			backgroundSource.Play ();
		} else {
			Sound s = sounds.Where (x => x.name == "game_background").First ();            
            backgroundSource.clip = s.clip;
			backgroundSource.volume = StageManager.instance.playerData.BackgroundMusic ? s.volume : 0;
			backgroundSource.pitch = s.pitch;
			backgroundSource.Play ();
		}
	}
}

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume;

	[Range(0.1f, 3f)]
	public float pitch;

	[HideInInspector]
	public AudioSource source;
}
