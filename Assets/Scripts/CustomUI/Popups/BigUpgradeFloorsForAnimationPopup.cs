using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigUpgradeFloorsForAnimationPopup : MonoBehaviour
{
    private Animator anim;
    private string nameToShow;
    private string valueToShow;
    private Sprite iconToShow;

    [SerializeField]
    private Text uiText;
    [SerializeField]
    private Image icon;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Show(string name, string value, Sprite icon)
    {
        this.nameToShow = name;
        this.valueToShow = value;
        this.iconToShow = icon;

        Show();
    }

    private void Show()
    {
        icon.sprite = iconToShow;
        uiText.text = LocalizationManager.instance.StringForKey(nameToShow);

        anim.SetTrigger("Show");
    }

    public void RefreshParams()
    {
        uiText.text = valueToShow;
    }
}
