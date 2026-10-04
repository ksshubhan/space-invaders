using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;

    public Web Web;
    public SceneLoader sceneLoader;

    void Awake()
    {
        Instance = this;
        Web = GetComponent<Web>();
        sceneLoader = GetComponent<SceneLoader>();
    }

    
}
