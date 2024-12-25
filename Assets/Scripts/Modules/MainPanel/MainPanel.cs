using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoSingleton<MainPanel>
{
    public ProgressView progress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
