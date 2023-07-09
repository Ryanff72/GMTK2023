using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
	public static FMODEvents instance { get; private set; }

	[field: Header("Music")]
	[field: SerializeField] public EventReference music { get; private set; }


	[field: Header("SFX")]
	[field: SerializeField] public EventReference newPotion { get; private set; }
	[field: SerializeField] public EventReference objectBounce { get; private set; }
	[field: SerializeField] public EventReference pageTurn { get; private set; }
	[field: SerializeField] public EventReference potionPoof { get; private set; }
	[field: SerializeField] public EventReference doorOpen { get; private set; }
	[field: SerializeField] public EventReference cauldronSplash { get; private set; }
	[field: SerializeField] public EventReference glassBounceClink { get; private set; }
	[field: SerializeField] public EventReference pouchOpen { get; private set; }
	[field: SerializeField] public EventReference potSFX { get; private set; }
	[field: SerializeField] public EventReference heaterSFX { get; private set; }
	[field: SerializeField] public EventReference metalThingSFX { get; private set; }


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}
}


