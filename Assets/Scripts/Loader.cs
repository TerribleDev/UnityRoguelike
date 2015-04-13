using System;
using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{

    public GameObject gameManager;

    void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
