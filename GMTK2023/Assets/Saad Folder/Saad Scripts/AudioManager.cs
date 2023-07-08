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
	PLAYBACK_STATE currentPlaybackstate;

	public static bool hasInititatedMusic;

	private Bus musicBus;


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

		SwitchTracks(scene);

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
		//print(currentPlaybackstate);

		musicBus.setVolume(musicVolume);

		#region Old Code
		////If the music has stopped, then start a new track 
		//if(currentPlaybackstate.Equals(PLAYBACK_STATE.STOPPED))
		//{
		//	SetMusic(0, currentPlaybackstate);
		//}
		#endregion

	}



	void SwitchTracks(Scene scene)
	{
		if(scene.buildIndex == 1)
		{
			if(!currentPlaybackstate.Equals(PLAYBACK_STATE.PLAYING))
				IntializeMusic(FMODEvents.instance.castleTheme);

		}

		if(scene.buildIndex == 0)
		{
			musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

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

		DontDestroyOnLoad(gameObject);
	}

	public void IntializeMusic(EventReference musicEventReference)
	{
		musicEventInstance = CreateEventInstance(musicEventReference);
		musicEventInstance.start();
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
