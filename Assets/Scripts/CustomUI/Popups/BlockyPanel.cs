using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockyPanel : MonoBehaviour
{
    [SerializeField]
    private Text blockyPanel_Text;

    private void Start()
    {
        blockyPanel_Text.text = LocalizationManager.instance.StringForKey("BlockyPanelText");
    }
}