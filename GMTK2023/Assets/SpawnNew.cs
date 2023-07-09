using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class SpawnNew : MonoBehaviour
{

    Animator anim ;
    GameObject go;
    public GameObject cauldronPoof;


    public Sprite[] sp;
    public Vector3 spsc;

    Sprite chosenSprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void SpawnItem (GameObject spawnedItem)
    {
        go = spawnedItem;

        //make for each item that can be added to the pot
        for(int i = 0; i < sp.Length; i++)
        {
            if (spawnedItem.name == sp[i].name)
            {
                chosenSprite = sp[i];
            }
            
        }
        if (chosenSprite == null)
        {
            chosenSprite = sp[0];
        }

        StartCoroutine("animAndSpawn");
    }


    public IEnumerator animAndSpawn()
    {
        transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = chosenSprite;
        transform.GetChild(0).transform.GetChild(0).transform.localScale = spsc;
        anim.Play("NewItem");
        Instantiate(cauldronPoof, new Vector3(-1.34000003f, -1.98000002f, -0.00999999978f), Quaternion.Euler(-90, 0, 0));
        yield return new WaitForSeconds(1.5f);
        anim.Play("Idle");
        Instantiate(go, new Vector2(4, 1.8f), Quaternion.identity);
    }
}
