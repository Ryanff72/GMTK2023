using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShiftUtility : MonoBehaviour
{
    public enum activeUtility { Kettle, Heater, Shaker };
    public activeUtility activeUtil;

    public GameObject kettlego;
    public GameObject heatergo;
    public GameObject shakergo;

    float firstStageSwapTime = 0.2f;
    float secondStageSwapTime = 0.35f;
    public float swapSpeed;


    public void shiftUtility (string utilityName)
    {
        firstStageSwapTime = 0.2f;
        secondStageSwapTime = 0.2f;
        if(utilityName == "kettle")
        {
            activeUtil = activeUtility.Kettle;
        }
        if (utilityName == "heater")
        {
            activeUtil = activeUtility.Heater;
        }
        if (utilityName == "shaker")
        {
            activeUtil = activeUtility.Shaker;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    public void StateMachine()
    {
        switch (activeUtil)
        {
            case activeUtility.Kettle:
                kettleActive();
                break;
            case activeUtility.Heater:
                heaterActive();
                break;
            case activeUtility.Shaker:
                shakerActive();
                break;
        }
    }

    void kettleActive()
    {
        if (firstStageSwapTime > 0)
        {
            firstStageSwapTime -= Time.deltaTime;
            kettlego.transform.position = Vector3.Lerp(kettlego.transform.position, new Vector3(-1.34000003f, -0.81f, 0.0367328897f), Time.deltaTime * swapSpeed);
            heatergo.transform.position = Vector3.Lerp(heatergo.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
            shakergo.transform.position = Vector3.Lerp(shakergo.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
        }
        else if(secondStageSwapTime > 0)
        {
            secondStageSwapTime -= Time.deltaTime;
            kettlego.transform.position = Vector3.Lerp(kettlego.transform.position, new Vector3(-1.34000003f, -1.49000001f, 0.0367328897f), Time.deltaTime * swapSpeed);
            heatergo.transform.position = Vector3.Lerp(heatergo.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
            shakergo.transform.position = Vector3.Lerp(shakergo.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
        }
    }

    void heaterActive()
    {
        if (firstStageSwapTime > 0)
        {
            firstStageSwapTime -= Time.deltaTime;
            kettlego.transform.position = Vector3.Lerp(kettlego.transform.position, new Vector3(-1.34526443f, -6.65f, 0.0367328897f), Time.deltaTime * swapSpeed);
            heatergo.transform.position = Vector3.Lerp(heatergo.transform.position, new Vector3(-1.34526443f, -1.7f, 0.0367328897f), Time.deltaTime * swapSpeed);
            shakergo.transform.position = Vector3.Lerp(shakergo.transform.position, new Vector3(-1.34526443f, -6.65f, 0.0367328897f), Time.deltaTime * swapSpeed);
        }
        else if (secondStageSwapTime > 0)
        {
            secondStageSwapTime -= Time.deltaTime;
            kettlego.transform.position = Vector3.Lerp(kettlego.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
            heatergo.transform.position = Vector3.Lerp(heatergo.transform.position, new Vector3(-1.34526443f, -2.3f, 0.0367328897f), Time.deltaTime * swapSpeed);
            shakergo.transform.position = Vector3.Lerp(shakergo.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
        }
    }

    void shakerActive()
    {
        if (firstStageSwapTime > 0)
        {
            firstStageSwapTime -= Time.deltaTime;
            kettlego.transform.position = Vector3.Lerp(kettlego.transform.position, new Vector3(-1.34526443f, -6.65f, 0.0367328897f), Time.deltaTime * swapSpeed);
            heatergo.transform.position = Vector3.Lerp(heatergo.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
            shakergo.transform.position = Vector3.Lerp(shakergo.transform.position, new Vector3(-1.34526443f, -1.7f, 0.0367328897f), Time.deltaTime * swapSpeed);
        }
        else if (secondStageSwapTime > 0)
        {
            secondStageSwapTime -= Time.deltaTime;
            kettlego.transform.position = Vector3.Lerp(kettlego.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
            heatergo.transform.position = Vector3.Lerp(heatergo.transform.position, new Vector3(-1.34526443f, -6.6500001f, 0.0367328897f), Time.deltaTime * swapSpeed);
            shakergo.transform.position = Vector3.Lerp(shakergo.transform.position, new Vector3(-1.34526443f, -2.3f, 0.0367328897f), Time.deltaTime * swapSpeed);
        }
    }


}
