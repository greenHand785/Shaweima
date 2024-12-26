using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class END : MonoBehaviour
{


    public GameObject MAIN_UI2;
    public GameObject MAIN_UI;
    public Button Restart2;
    public Button MainUI;

    public TextMeshProUGUI Year;
    void Start()
    {
        Restart2.onClick.AddListener(Restart);
        MainUI.onClick.AddListener(MainManue);
    }

    // Update is called once per frame
    void Update()
    {
        Year.text="存活："+GameManager.Instance.curLevel+"年";
    }


    public void Restart()
    {
        gameObject.SetActive(false);
        MAIN_UI2.SetActive(true);
        MAIN_UI.SetActive(true);
        //todoR
    }

    public void MainManue()
    {
        gameObject.SetActive(false);
    }
    
    
}
