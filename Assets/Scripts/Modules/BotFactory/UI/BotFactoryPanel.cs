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

    private bool isCreated;
    // Start is called before the first frame update
    void Start()
    {
        closeBtn = transform.Find("Close").GetComponent<Button>();
        botRoot = transform.Find("Scroll View/Viewport/Content").transform;
        botExample = transform.Find("Scroll View/Viewport/Content/Example").transform;
        botExample.gameObject.SetActive(false);
        closeBtn.onClick.AddListener(OnClickClose);
    }

    // Update is called once per frame
    void Update()
    {
        CreateBotInfo();
    }

    void OnClickClose()
    {
        gameObject.SetActive(false);
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
}
