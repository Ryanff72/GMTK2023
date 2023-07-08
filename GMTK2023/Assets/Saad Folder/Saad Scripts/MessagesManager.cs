using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

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
	[SerializeField] private int currentNumOfLines;
	private GameObject spawnedTextMessage;
	int numberOfLineTyping;

	[Header("Typing Variables")]
	float preferredHeight;
	int numberOfLines;
	float positionOffset, scaleOffset;
	float startYPos, startYScale;
	bool isTyping = false;
	bool hasStartedBubble = false;
	bool hasFinishedDialogue = false;
	Coroutine currentTypingCoroutine;

	//raycast variables
	Camera cam;
	RaycastHit2D hitInfo;

	private void Start()
	{
		cam = Camera.main;
	}

	private void Update()
	{
		//if(Input.GetKeyDown(KeyCode.Mouse0) && !hasStartedBubble)
		//{
		//	SpawnTextMessageAndStartDialogue(currentSpeaker);
		//}

		if(!hasFinishedDialogue && hasStartedBubble && currentRenderer != null) 
			UpdateTextBox();

		if (Input.GetButtonDown("Fire1") /*&& !isTyping*/)
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
			if(hitInfo.collider != null && hitInfo.collider.CompareTag("TextBox"))
			{
				NextSentence();
			}

		}
	}


	public void SpawnTextMessageAndStartDialogue(Customer dialogueObject)
	{
		currentSpeaker = dialogueObject;

		hasFinishedDialogue = false;
		hasStartedBubble = true;
		Vector2 spawnPos = Vector2.one;

		//sets the initial position of the message and instantiates it
		spawnPos = new Vector2(messageSpawnPos.localPosition.x, messageSpawnPos.localPosition.y);
		spawnedTextMessage = Instantiate(messagePrefab, spawnPos, Quaternion.identity, messagesParent);

		

		//setst the scale
		currentRenderer = spawnedTextMessage.GetComponentInChildren<SpriteRenderer>();
		Transform spriteTransform = currentRenderer.transform;
		startYPos = currentRenderer.transform.localPosition.y;
		startYScale = currentRenderer.transform.localScale.y;

		//Color boxColour = new Color(currentSpeaker.boxColour.r, currentSpeaker.boxColour.g, currentSpeaker.boxColour.b, 255f);
		//currentRenderer.color = boxColour;

		//Sets the text of the message
		currentText = spawnedTextMessage.GetComponentInChildren<TextMeshProUGUI>();
		currentTypingCoroutine = StartCoroutine(Type(currentSpeaker));

	}

	IEnumerator Type(Customer currentCharacter)
	{
		//Sets up the text box for the first time
		currentText.text = "";
		currentNumOfLines = currentCharacter.dialogueLines.Length;
		numberOfLineTyping = Mathf.Clamp(numberOfLineTyping, 0, currentCharacter.dialogueLines.Length - 1);

		foreach (char letter in currentCharacter.dialogueLines[numberOfLineTyping].ToCharArray())
		{
			currentText.text += letter;
			isTyping = true;
			//calculates the number of lines for each message 


			yield return new WaitForSeconds(currentCharacter.typingInterval);
			if(currentText.text == currentSpeaker.dialogueLines[numberOfLineTyping])
			{
				isTyping = false;
				//Then the text has reached its end
				if(numberOfLineTyping == currentCharacter.dialogueLines.Length - 1)
				{
					print("finished dialogue");
					//reached end of the entire dialogue and so do nothing
					hasFinishedDialogue = true;
					hasStartedBubble = false;
					StopCoroutine(currentTypingCoroutine);



				}
			}
		}
	}

	void NextSentence()
	{
		
		if(hasFinishedDialogue || (numberOfLineTyping == currentSpeaker.dialogueLines.Length - 1))
		{
			numberOfLineTyping = 0;
			Destroy(spawnedTextMessage);
		}
		StopCoroutine(currentTypingCoroutine);
		numberOfLineTyping++;
		if(currentTypingCoroutine != null)
			currentTypingCoroutine = StartCoroutine(Type(currentSpeaker));
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


