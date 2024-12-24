using Component.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotFactoryPanel : MonoBehaviour
{
    private Button closeBtn;

    private Transform botRoot;
    private Transform botExample;


    private Animation animations;
    private bool isCreated;

    public bool state;
    // Start is called before the first frame update
    void Start()
    {
        animations = transform.GetComponent<Animation>();
        closeBtn = transform.Find("Close").GetComponent<Button>();
        botRoot = transform.Find("Scroll View/Viewport/Content").transform;
        botExample = transform.Find("Scroll View/Viewport/Content/Example").transform;
        botExample.gameObject.SetActive(false);
        closeBtn.onClick.AddListener(OnClickClose);
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        CreateBotInfo();
    }

    void OnClickClose()
    {
        Close();
    }

    private void CreateBotInfo()
    {
        if (isCreated)
        {
            return;
        }
        isCreated = true;
        foreach (var item in BotFactory.Instance.GetBotExamples())
        {
            BotProductInfoPanel info = Instantiate(botExample.gameObject, botRoot).GetComponent<BotProductInfoPanel>();
            info.SetBotProductInfo(item.Key, item.Value);
            info.gameObject.SetActive(true);
        }
    }

    public void Open()
    {
        animations.Play("BotFactoryPanelOpen");
        state = true;
    }

    public void Close()
    {
        animations.Play("BotFactoryPanelClose");
        state = false;
    }
}
