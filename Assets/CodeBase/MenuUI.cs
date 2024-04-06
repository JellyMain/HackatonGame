using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    private ISceneLoaderService sceneLoaderService;
    
    
    private void Awake()
    {
        sceneLoaderService = AllServices.Container.Single<ISceneLoaderService>();
    }


    public void StartGame()
    {
        sceneLoaderService.LoadScene(2);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
