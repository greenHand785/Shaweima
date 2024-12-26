using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotProductInfoPanel : MonoBehaviour
{
    private Text nameTxt;
    private Text levelTxt;
    private Text createTotalTime; // �����ܺ�ʱs
    private Text moveSpeed; // �ƶ��ٶ�
    private Text effect; // Ч��
    private Text cost; // ��������һ�����Ľ������
    private Text durability; // �;ö�

    private Text levelTxt2;
    private Text createTotalTime2; // �����ܺ�ʱs
    private Text moveSpeed2; // �ƶ��ٶ�
    private Text effect2; // Ч��
    private Text cost2; // ��������һ�����Ľ������
    private Text durability2; // �;ö�

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
        levelTxt.text = "�ȼ�:" + product.info.level.ToString();
        createTotalTime.text = "���ɳ��ܺ�ʱ:" + product.info.createTotalTime.ToString();
        moveSpeed.text = "�ƶ��ٶ�:" + product.info.moveSpeed.ToString();
        effect.text = "����Ч��:" + product.info.effect.ToString();
        cost.text = "�����ķѽ��:" + product.info.cost.ToString();
        durability.text = "�;ö�:" + product.info.durability.ToString();
        nameTxt.text = type.ToString();
        JsonProductDefield info = BotFactory.Instance.GetJsonProductInfo(product.levelId, product.info.level + 1);
        if(info != null)
        {
            levelTxt2.text = "�ȼ�:" + info.level.ToString();
            createTotalTime2.text = "���ɳ��ܺ�ʱ:" + info.createTotalTime.ToString();
            moveSpeed2.text = "�ƶ��ٶ�:" + info.moveSpeed.ToString();
            effect2.text = "����Ч��:" + info.effect.ToString();
            cost2.text = "�����ķѽ��:" + info.cost.ToString();
            durability2.text = "�;ö�:" + info.durability.ToString();
        }
        // ����ʱ
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
