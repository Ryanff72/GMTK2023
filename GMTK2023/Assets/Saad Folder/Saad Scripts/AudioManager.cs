using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
	[Header("Volume")]
	[Range(0, 1)]
	public float musicVolume = 1f;
	[Range(0, 1)]
	public float sfxVolume = 1f;


	PLAYBACK_STATE currentPlaybackstate;
	[HideInInspector]public static bool hasInititatedMusic;

	private Bus musicBus;
	private Bus sfxBus;


	private List<EventInstance> eventInstances;

	public static AudioManager instance { get; private set; }

	private EventInstance musicEventInstance;

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//does something based on the scene
	}

	// called when the game is terminated
	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Update()
	{
		//Gets the current playback state
		musicEventInstance.getPlaybackState(out currentPlaybackstate);
		print(currentPlaybackstate);

		musicBus.setVolume(musicVolume);
		sfxBus.setVolume(sfxVolume);

		#region Old Code
		////If the music has stopped, then start a new track 
		//if(currentPlaybackstate.Equals(PLAYBACK_STATE.STOPPED))
		//{
		//	SetMusic(0, currentPlaybackstate);
		//}
		#endregion

	}


	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		eventInstances = new List<EventInstance>();

		musicBus = RuntimeManager.GetBus("bus:/Music");
		sfxBus = RuntimeManager.GetBus("bus:/SFX");

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		IntializeMusic(FMODEvents.instance.music);
		//AudioManager.instance.PlayOneShot(FMODEvents.instance.newPotion, transform.position);
	}

	public void IntializeMusic(EventReference musicEventReference)
	{
		musicEventInstance = CreateEventInstance(musicEventReference);
		musicEventInstance.start();
	}

	public void SetMusic(MusicEnum musicType)
	{
		musicEventInstance.setParameterByName("MusicType", (int)musicType);
	}
	public void PlayOneShot(EventReference sound, Vector3 worldPos)
	{
		RuntimeManager.PlayOneShot(sound, worldPos);
	}

	#region Old Code
	//public void SetMusic(int musicValue, PLAYBACK_STATE currentState)
	//{
	//	musicEventInstance.setParameterByName("track", musicValue);
	//	//If the music has stopped then initialize the music again
	//	if(currentState.Equals(PLAYBACK_STATE.STOPPED))
	//	{
	//		IntializeMusic(FMODEvents.instance.music);
	//		musicEventInstance.setParameterByName("track", musicValue);
	//	}
	//}
	#endregion

	public EventInstance CreateEventInstance(EventReference eventReference)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		eventInstances.Add(eventInstance);
		return eventInstance;
	}

	private void CleanUp()
	{
		if(eventInstances != null)
		{
			foreach (EventInstance e in eventInstances)
			{
				e.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				e.release();
			}
		}


	}

	private void OnDestroy()
	{
		CleanUp();
	}
}
