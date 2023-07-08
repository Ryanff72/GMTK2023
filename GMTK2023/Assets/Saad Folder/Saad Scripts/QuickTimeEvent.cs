using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEvent : MonoBehaviour
{
	[SerializeField] private QuickTimePrompt[] qtePrompts;
	[SerializeField] private Transform qteSpawnPos;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			RandomQTE();
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
			StartCoroutine(StartQTE(spawnedKey,qtePrompts[0].keyCode));
		}
	}

	IEnumerator StartQTE(GameObject spawnedObject , KeyCode keyCode)
	{
		if(Input.inputString.Contains(keyCode.ToString())) 
		{
			//the person hit the thingie this frame 
			yield return null;

		}
	}
}
[System.Serializable]
public struct QuickTimePrompt
{
	public KeyCode keyCode;
	public GameObject qtePrefab;
}
