using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWorkgroundToCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Camera modelCamera;

    const float defaultTile = (float)16 / 9;
    void Start()
    {
        FitOrthographic();
        FitPerspective();
    }
    [Button]
    void FitPerspective()
    {
        float fov = (float)Screen.height / Screen.width;
        if (fov > defaultTile)
        {
            modelCamera.fieldOfView = (fov / defaultTile - 1) * 30 + 70;
        }
        else
        {
            modelCamera.fieldOfView =  70;

        }
    }

    void FitOrthographic()
    {
        mainCamera.orthographicSize = 1;
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height));
        //Debug.Log(mainCamera.ViewportToScreenPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height)));
        Vector3 screenSize = topRight - bottomLeft;
        //Debug.Log(new Vector3(mainCamera.rect.width, mainCamera.rect.height));
        //Debug.Log(topRight.x+ " " +bottomLeft);
        float screenRatio = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;
        //Debug.Log(screenRatio);
        //Debug.Log(desiredRatio);

        if (screenRatio >= desiredRatio)
        {
            mainCamera.orthographicSize = 15;
        }
        else
        {
            float x = topRight.x;
            mainCamera.orthographicSize = 15 * (0.5625f / x);
        }

    }
}
