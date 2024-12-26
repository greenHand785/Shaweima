using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Reflection;
using JetBrains.Annotations;
using UnityEngine;

public class ESC : MonoBehaviour
{
    public GameObject MainUI;
    void Start()
    {
          GameManager.Instance.ESC_UI=gameObject;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }  


    }

    public void Quit()
    {
       if (MainUI.activeSelf==false) 
            GameManager.Instance.isStartGame=true;
         gameObject.SetActive(false);
    }
  
}
