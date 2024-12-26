using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoSingleton<MainPanel>
{
    public ProgressView progress;
    public Text goldTxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goldTxt.text = GoldSystem.Instance.Gold.ToString();
    }

    public void Init(float hp)
    {
        progress.maxValue = hp;
        progress.SetValueIm(hp);
    }

    public void SetHPValue(float value)
    {
        progress.Value = value;
    }
}
