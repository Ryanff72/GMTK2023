using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Customer : ScriptableObject
{
	public new string name;

	[Header("Texting Variables")]
	public string[] dialogueLines;
	public float typingInterval = 0.05f;
	public Color boxColour;
	public EventReference letterSound;


}
