using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public enum shakerStatus { Shaking, NotShaking, Dormant, Output };
    public shakerStatus shksts;

    Vector3 worldMousePos;
    bool hovering;

    float Xtraveled;
    float Ytraveled;

    float lastPosX;
    float lastPosY;

    float totaltraveled;
    public float shakeRequired;

    private void Start()
    {
        shksts = shakerStatus.Dormant;
    }
    public void StateMachine()
    {
        switch (shksts)
        {
            case shakerStatus.Dormant:
                break;


            case shakerStatus.NotShaking:
                goBack();
                break;
            case shakerStatus.Shaking:
                shakeIngredients();
                break;
            case shakerStatus.Output:
                goBack();
                break;
        }
    }

    private void Update()
    {
        StateMachine();

        //mousepos to worldpos
        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        worldMousePos = Camera.main.ScreenToWorldPoint(screenPoint);

        //pick up the object
        if (Input.GetButtonDown("Fire1") && hovering)
        {
            shksts = shakerStatus.Shaking;
        }
        if (Input.GetButtonUp("Fire1") && shksts == shakerStatus.Shaking)
        {
            shksts = shakerStatus.NotShaking;
        }
    }
    private void OnMouseEnter()
    {
        hovering = true;
    }
    private void OnMouseExit()
    {
        hovering = false;
    }


    void goBack()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(-1.34526443f, -0.74000001f, 0.0367328897f), Time.deltaTime * 30);
    }

    void shakeIngredients()
    {
        if (GetComponentInChildren<BoxCollider2D>().enabled)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        //sees if the thing has travelled far enough
        Xtraveled += Mathf.Abs(transform.position.x - lastPosX);
        lastPosX = transform.position.x;
        Ytraveled += Mathf.Abs(transform.position.y - lastPosY);
        lastPosY = transform.position.y;

        if (Xtraveled + Ytraveled >= shakeRequired)
        {
            shksts = shakerStatus.Output;
            Xtraveled = 0.0f;
            Ytraveled = 0.0f;
        }
        transform.position = worldMousePos;
    }



}
