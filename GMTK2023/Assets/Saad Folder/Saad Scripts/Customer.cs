using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Customer : ScriptableObject
{
	[SerializeField] private new string name;
	[SerializeField] private string[] dialogueLines;
	[SerializeField] private PotionTypes preferredPotionType;



}
