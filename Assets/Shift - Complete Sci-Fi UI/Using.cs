using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
public class Using : MonoBehaviour
{
   public GameObject MainUI;
  public GameObject[] pages;

    // 当前显示的页面索引
    private int currentPageIndex = 0;

    // 翻页按钮
    public Button nextPageButton;     // 下一页按钮
    public Button previousPageButton; // 上一页按钮

    public void Open()  
    {
        GameManager.Instance.isStartGame=false;    
    }
    public void Quit()
    {
        gameObject.SetActive(false);
        if (MainUI.active!=true)  GameManager.Instance.isStartGame=true;    

      
    }
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
            }  
    }
    void Start()
    {
         GameManager.Instance.Usin_UI=gameObject;
        // 注册按钮点击事件
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);
        // 初始化页面和按钮状态
        UpdatePageUI();
    }

    /// <summary>
    /// 切换到下一页
    /// </summary>
    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1) // 防止越界
        {
            currentPageIndex++;
            UpdatePageUI();
        }
    }

    /// <summary>
    /// 切换到上一页
    /// </summary>
    public void PreviousPage()
    {
        if (currentPageIndex > 0) // 防止越界
        {
            currentPageIndex--;
            UpdatePageUI();
        }
    }

    /// <summary>
    /// 更新页面显示和按钮状态
    /// </summary>
    private void UpdatePageUI()
    {
        // 遍历所有页面，只有当前页激活
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPageIndex);
        }

        // 更新按钮状态
        if (previousPageButton != null)
        {
            previousPageButton.gameObject.SetActive(true); // 确保按钮显示
            previousPageButton.interactable = currentPageIndex > 0; // 如果是第一页，只禁用按钮
        }

        if (nextPageButton != null)
        {
            // 在最后一页隐藏“向后”按钮
                //nextPageButton.gameObject.SetActive(currentPageIndex < pages.Length - 1);
                nextPageButton.interactable=currentPageIndex < pages.Length - 1;
        }
    }
}
