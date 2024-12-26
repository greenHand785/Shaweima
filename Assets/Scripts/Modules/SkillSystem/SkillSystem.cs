using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// 技能系统
/// 3种技能
/// 每种技能可以升级，并且技能有对应的
/// 攻击特效，ui特效，升级特效
/// 
/// </summary>
public class SkillSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button upgrade_Button;
    public Button skill_Button;
    public Image skillImage;
    public Image timerMask;
    public TextMeshProUGUI skillLevel_Text;
    public GameObject skillDetailPanel;

    public JsonSkillInfo info;
    public GameObject skillPrefab;
    public int moneyCount;
    public float CD = 5;

    //当前技能冷却值
    private float currentCD;
    //当前技能等级
    private int currentLevel = 1;
    //
    private float skillHarm;

    private Ray _ray;
    private RaycastHit _raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        upgrade_Button.onClick.AddListener(UpgradeSkill);
        skill_Button.onClick.AddListener(UseSkill);

        skillLevel_Text.text = currentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        moneyCount = (int)GoldSystem.Instance.Gold;

        //技能CD冷却
        if (currentCD > 0)
        {
            currentCD -= Time.fixedDeltaTime;
            timerMask.fillAmount = currentCD / CD;
        }
        else
        {
            currentCD = 0;
            timerMask.fillAmount = 0;
            skill_Button.interactable = true;
        }

        //判断金币是否够升级技能
        if (info != null && info.skillUpgradeCost.Length > 0)
        {
            if (skillLevel_Text.text == "Max") return;

            if (currentLevel > info.skillUpgradeCost.Length)
            {
                skillLevel_Text.text = "Max";
                return;
            }

            if (moneyCount >= info.skillUpgradeCost[currentLevel - 1] && upgrade_Button.gameObject.activeSelf == false)
            {
                upgrade_Button.gameObject.SetActive(true);
            }
        }
    }

    void UseSkill()
    {
        if (currentCD > 0 && GameManager.Instance.currentSkillObj != null)
        {
            return;
        }
        else
        {
            currentCD = CD;

            timerMask.fillAmount = 1;
            skill_Button.interactable = false;

            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _raycastHit, 1000f))
            {
                GameObject skill = Instantiate(skillPrefab, _raycastHit.point, Quaternion.identity);
                skill.GetComponent<SkillItem>().skillHarm = skillHarm;
                GameManager.Instance.currentSkillObj = skill;

                GameObject skillItem = Instantiate(GameManager.Instance.skillItemPrefab, _raycastHit.point, Quaternion.identity);
                GameManager.Instance.skillItem = skillItem;
            }
        }
    }

    void UpgradeSkill()
    {
        if (info.skillUpgradeCost.Length > 0)
        {
            if (moneyCount >= info.skillUpgradeCost[currentLevel - 1])
            {
                GoldSystem.Instance.SubGold(info.skillUpgradeCost[currentLevel - 1]);
                currentLevel++;
                skillLevel_Text.text = currentLevel.ToString();
                upgrade_Button.gameObject.SetActive(false);
                skillHarm += 15;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillDetailPanel == null) return;

        Debug.Log(eventData.pointerEnter.gameObject.transform.parent.parent.name);

        //获取当前鼠标进入对象的名称做比对
        if (eventData.pointerEnter.gameObject.transform.parent.parent.name.Contains("Skill_"))
        {
            skillDetailPanel.SetActive(true);//显示信息面板

            skillDetailPanel.transform.Find("Image/SkillName").GetComponent<TextMeshProUGUI>().text = info.skillName;
            skillDetailPanel.transform.Find("Image/Detail").GetComponent<TextMeshProUGUI>().text = info.skillDes;
            skillDetailPanel.transform.Find("Image/Image").GetComponent<Image>().sprite = skillImage.sprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillDetailPanel.SetActive(false);
    }

    public void GetPrefabInfo()
    {
        skillPrefab = Resources.Load<GameObject>(info.skillPrefaber);
        skillImage.sprite = Resources.Load<Sprite>(info.skillIcon);
        skillHarm = info.skillHarm;
        CD = info.CD;
    }
}
