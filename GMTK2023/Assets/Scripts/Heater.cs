using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : MonoBehaviour
{
    public List<GameObject> collidedObjects;
    public List<float> cookTimes;
    public float cookTime;
    public GameObject smokeEffect;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < collidedObjects.Count; i++)
        {
            if (cookTimes[i] == -1)
            {
                continue;
            }
            cookTimes[i] += 0.1f;
            if (cookTimes[i] > cookTime)
            {
                cookTimes[i] = -1;
                heatItem(collidedObjects[i]);
            }
        }
    }


    void heatItem(GameObject heatedObject)
    {
        heatedObject.GetComponent<Ingredient>().Heat();
        Instantiate(smokeEffect, heatedObject.transform.position, Quaternion.Euler(-90, 0, 0));
        audioManager.PlaySoundEffect("donecooking",0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collidedObjects.Contains(collision.collider.gameObject) && collision.collider.gameObject.GetComponent<Ingredient>() != null)
        {
            collidedObjects.Add(collision.collider.gameObject);
            cookTimes.Add(0.0f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collidedObjects.Contains(collision.collider.gameObject) && collision.collider.gameObject.GetComponent<Ingredient>() != null)
        {
            int index = collidedObjects.IndexOf(collision.collider.gameObject);
            collidedObjects.Remove(collision.collider.gameObject);
            cookTimes.RemoveAt(index);
        }
    }
}
