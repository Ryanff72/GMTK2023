using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEvent : MonoBehaviour
{
	[SerializeField] private QuickTimePrompt[] qtePrompts;
	[SerializeField] private Transform qteSpawnPos;

	Coroutine currentQTECoroutine;
	KeyCode currentKeyCode;
	GameObject currentQTEObject;
	float timeToQTE = 1f;
	bool canQTE;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			RandomQTE();
		}

		if(currentQTECoroutine != null)
		{
			CheckIfQTE();
		}
	}

	void RandomQTE()
	{
		//Instantiate(qtePrompts[0].qtePrefab, qteSpawnPos.position, Quaternion.identity);


		int randomValue = Random.Range(1, 6);
		if (randomValue == 1)
		{
			//this has a 20% of occuring
			GameObject spawnedKey = Instantiate(qtePrompts[0].qtePrefab, qteSpawnPos.position, Quaternion.identity);
			currentQTECoroutine = StartCoroutine(StartQTE(spawnedKey,qtePrompts[0].keyCode));
		}
	}

	void CheckIfQTE()
	{
		if(canQTE == true && Input.GetKeyDown(currentKeyCode))
		{
			//succesfully did QTE
			Destroy(currentQTEObject);
		}
		else if(canQTE == false)
		{
			//failed QTE
			print("Failed QTE");
			StopCoroutine(currentQTECoroutine);
		}
	}

	IEnumerator StartQTE(GameObject spawnedObject , KeyCode keyCode)
	{
		currentKeyCode = keyCode;
		currentQTEObject = spawnedObject;
		canQTE = true;
		yield return new WaitForSeconds(timeToQTE);
		canQTE = false;
	}
}
[System.Serializable]
public struct QuickTimePrompt
{
	public KeyCode keyCode;
	public GameObject qtePrefab;
}
