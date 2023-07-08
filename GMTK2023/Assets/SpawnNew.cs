using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class SpawnNew : MonoBehaviour
{

    Animator anim ;
    GameObject go;
    Sprite sp;
    Vector3 spsc;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void SpawnItem (GameObject spawnedItem, Sprite AppearanceSprite, Vector3 spriteScale)
    {
        go = spawnedItem;
        sp = AppearanceSprite;
        spsc = spriteScale;
        StartCoroutine("animAndSpawn");
    }


    public IEnumerator animAndSpawn()
    {
        transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sp;
        transform.GetChild(0).transform.GetChild(0).transform.localScale = spsc;
        anim.Play("NewItem");
        yield return new WaitForSeconds(1.5f);
        anim.Play("Idle");
        Instantiate(go, new Vector2(4, 1.8f), Quaternion.identity);
    }
}
