using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Customer : ScriptableObject
{
	 public new string name;
	 public string[] dialogueLines;
	 public PotionTypes preferredPotionType;



}
