using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotProductInfoPanel : MonoBehaviour
{
    private Text nameTxt;
    private Text levelTxt;
    private Text createTotalTime; // 生成总耗时s
    private Text moveSpeed; // 移动速度
    private Text effect; // 效率
    private Text cost; // 升级到下一级消耗金币数量
    private Text durability; // 耐久度

    private Text levelTxt2;
    private Text createTotalTime2; // 生成总耗时s
    private Text moveSpeed2; // 移动速度
    private Text effect2; // 效率
    private Text cost2; // 升级到下一级消耗金币数量
    private Text durability2; // 耐久度

    private Button createBtn;
    private Button upBtn;

    private Text time;

    public Image iconImg;
    public Image upImg;
    public GameObject Mask;
    private BotFactoryInfoBase product;
    private ObjectType type;
    // Start is called before the first frame update
    void Start()
    {
        nameTxt = transform.Find("Name").GetComponent<Text>();
        levelTxt = transform.Find("PropertyRoot/Property/property1").GetComponent<Text>();
        createTotalTime = transform.Find("PropertyRoot/Property/property1 (1)").GetComponent<Text>();
        moveSpeed = transform.Find("PropertyRoot/Property/property1 (2)").GetComponent<Text>();
        effect = transform.Find("PropertyRoot/Property/property1 (3)").GetComponent<Text>();
        cost = transform.Find("PropertyRoot/Property/property1 (4)").GetComponent<Text>();
        durability = transform.Find("PropertyRoot/Property/property1 (5)").GetComponent<Text>();

        levelTxt2 = transform.Find("PropertyRoot/NextProperty/property1").GetComponent<Text>();
        createTotalTime2 = transform.Find("PropertyRoot/NextProperty/property1 (1)").GetComponent<Text>();
        moveSpeed2 = transform.Find("PropertyRoot/NextProperty/property1 (2)").GetComponent<Text>();
        effect2 = transform.Find("PropertyRoot/NextProperty/property1 (3)").GetComponent<Text>();
        cost2 = transform.Find("PropertyRoot/NextProperty/property1 (4)").GetComponent<Text>();
        durability2 = transform.Find("PropertyRoot/NextProperty/property1 (5)").GetComponent<Text>();
        time = transform.Find("Time").GetComponent<Text>();
        createBtn = transform.Find("icon").GetComponent<Button>();
        upBtn = transform.Find("up").GetComponent<Button>();

        createBtn.onClick.AddListener(OnClickCreateBtn);
        upBtn.onClick.AddListener(OnUpBtn);

    }

    // Update is called once per frame
    void Update()
    {
        levelTxt.text = "等级:" + product.info.level.ToString();
        createTotalTime.text = "生成充能耗时:" + product.info.createTotalTime.ToString();
        moveSpeed.text = "移动速度:" + product.info.moveSpeed.ToString();
        effect.text = "工作效率:" + product.info.effect.ToString();
        cost.text = "升级耗费金币:" + product.info.cost.ToString();
        durability.text = "耐久度:" + product.info.durability.ToString();
        nameTxt.text = type.ToString();
        JsonProductDefield info = BotFactory.Instance.GetJsonProductInfo(product.levelId, product.info.level + 1);
        if(info != null)
        {
            levelTxt2.text = "等级:" + info.level.ToString();
            createTotalTime2.text = "生成充能耗时:" + info.createTotalTime.ToString();
            moveSpeed2.text = "移动速度:" + info.moveSpeed.ToString();
            effect2.text = "工作效率:" + info.effect.ToString();
            cost2.text = "升级耗费金币:" + info.cost.ToString();
            durability2.text = "耐久度:" + info.durability.ToString();
        }
        // 倒计时
        float timeValue = product.info.createTotalTime - product.curTime;
        timeValue = Mathf.Max(0, timeValue);
        time.text = timeValue.ToString("F0");
        time.gameObject.SetActive(timeValue > 0);
        Mask.gameObject.SetActive(!product.isProduce);
        bool state = info != null;
        levelTxt2.gameObject.SetActive(state);
        createTotalTime2.gameObject.SetActive(state);
        moveSpeed2.gameObject.SetActive(state);
        effect2.gameObject.SetActive(state);
        cost2.gameObject.SetActive(state);
        durability2.gameObject.SetActive(state);
        //createBtn.gameObject.SetActive(product.isProduce);

        if(iconImg.sprite == null)
        {
            iconImg.sprite = Resources.Load<Sprite>(product.info.iconPath);
        }
        if (upImg.sprite == null)
        {
            upImg.sprite = Resources.Load<Sprite>(product.info.upIconPath);
        }
    }

    public void SetBotProductInfo(ObjectType type, BotFactoryInfoBase info)
    {
        product = info;
        this.type = type;
    }

    void OnClickCreateBtn()
    {
        BotFactory.Instance.CreateProduct(type);
    }

    void OnUpBtn()
    {
        BotFactory.Instance.UpProductLevel(type, GoldSystem.Instance);
    }
}
