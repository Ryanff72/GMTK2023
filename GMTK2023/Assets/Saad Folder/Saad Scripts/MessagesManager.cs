using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
	[SerializeField] private Customer currentSpeaker;

	[Header("Messages Setup Variables")]
	[SerializeField] private Transform messagesParent;
	[SerializeField] private GameObject messagePrefab;
	[SerializeField] private Transform messageSpawnPos;
	[SerializeField] private Vector2 positionToSizeYRatio = new Vector2(-13.54f, 26.92421f);
	[SerializeField] private const float preferredHeightInterval = 24.86f;
	[SerializeField] private TextMeshProUGUI currentText;
	[SerializeField] private SpriteRenderer currentRenderer;

	[Header("Typing Variables")]
	[SerializeField] private float timeBetweenLetters = 0.2f;
	float preferredHeight;
	int numberOfLines;
	float positionOffset, scaleOffset;

	float startYPos, startYScale;

	private void Start()
	{
		SpawnTextMessage();

	}

	void SpawnTextMessage()
	{
		Vector2 spawnPos = Vector2.one;

		//sets the initial position of the message and instantiates it
		spawnPos = new Vector2(messageSpawnPos.localPosition.x, messageSpawnPos.localPosition.y);
		GameObject spawnedTextMessage = Instantiate(messagePrefab, spawnPos, Quaternion.identity, messagesParent);

		

		currentRenderer = spawnedTextMessage.GetComponentInChildren<SpriteRenderer>();
		Transform spriteTransform = currentRenderer.transform;
		startYPos = currentRenderer.transform.localPosition.y;
		startYScale = currentRenderer.transform.localScale.y;

		//Sets the text of the message
		currentText = spawnedTextMessage.GetComponentInChildren<TextMeshProUGUI>();
		StartCoroutine(Type(currentSpeaker));







	}

	IEnumerator Type(Customer currentCharacter)
	{
		//Sets up the text box for the first time
		currentText.text = "";


		foreach (char letter in currentCharacter.dialogueLines[0].ToCharArray())
		{
			print("letter");
			currentText.text += letter;

			//calculates the number of lines for each message 


			yield return new WaitForSeconds(timeBetweenLetters);
		}
	}

	private void Update()
	{
		UpdateTextBox();
	}

	void UpdateTextBox()
	{
		preferredHeight = currentText.preferredHeight;
		numberOfLines = Mathf.RoundToInt(preferredHeight / preferredHeightInterval);

		positionOffset = positionToSizeYRatio.x * numberOfLines;
		scaleOffset = positionToSizeYRatio.y * numberOfLines;

		currentRenderer.transform.localPosition = new Vector2(currentRenderer.transform.localPosition.x, startYPos + positionOffset);
		currentRenderer.transform.localScale = new Vector2(currentRenderer.transform.localScale.x, startYScale + scaleOffset);

		

	}

}


