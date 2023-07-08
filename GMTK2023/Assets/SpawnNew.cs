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
    public Vector3[] spsc;

    Sprite chosenSprite;
    Vector3 chosenVector3;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void SpawnItem (GameObject spawnedItem)
    {
        go = spawnedItem;

        //make for each item that can be added to the pot
        if (spawnedItem.name == "veggiePaste")
        {
            //chosenSprite = sp[0];
            //chosenVector3 = spsc[0]; 
        }

        StartCoroutine("animAndSpawn");
    }


    public IEnumerator animAndSpawn()
    {
        transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = chosenSprite;
        transform.GetChild(0).transform.GetChild(0).transform.localScale = chosenVector3;
        anim.Play("NewItem");
        Instantiate(cauldronPoof, new Vector3(-1.34000003f, -1.98000002f, -0.00999999978f), Quaternion.Euler(-90, 0, 0));
        yield return new WaitForSeconds(1.5f);
        anim.Play("Idle");
        Instantiate(go, new Vector2(4, 1.8f), Quaternion.identity);
    }
}
