using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentEventSystem : MonoBehaviour
{
   private static PersistentEventSystem instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<PersistentEventSystem>();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != GetComponent<PersistentEventSystem>())
        {
            Destroy(gameObject);
        }
    }
}