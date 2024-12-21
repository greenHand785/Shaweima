using Component.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    private Text title;
    private Text content;

    private UIFollowObject follow;
    private string title_str;
    private string content_str;
    // Start is called before the first frame update
    void Start()
    {
        title = transform.Find("title").GetComponent<Text>();
        content = transform.Find("content").GetComponent<Text>();
        follow = transform.GetComponent<UIFollowObject>();
    }

    private void Update()
    {
        title.text = title_str;
        content.text = content_str;
    }
    public void SetTitle(string str)
    {
        title_str = str;
    }

    public void SetContent(string str)
    {
        content_str = str;
    }

    public void SetTarget(Transform target)
    {
        follow.target = target;
        follow.showCamera = UIMgr.Instance.UICamera;
    }
}
