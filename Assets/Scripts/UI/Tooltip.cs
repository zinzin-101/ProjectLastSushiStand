using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [Serializable] public class AnimationSettings
    {
        public enum OpenStyle { WidthToHeight, HeightToWidth, HightAndWidth };
        public OpenStyle openStyle;
        public float widthSmooth = 4.6f, heightSmooth = 4.6f;
        public float textSmooth = 0.1f;

        [HideInInspector] public bool widthOpen = false, heightOpen = false;

        public void Initialize()
        {
            widthOpen = false;
            heightOpen = false;
        }
    }

    [Serializable] public class UISettings
    {
        public Image textBox;
        public TMP_Text text;
        public Vector2 initialBoxSize = new Vector2(0.25f, 0.25f);
        public Vector2 openedBoxSize = new Vector2(400, 200);
        public float snapToSizeDistance = 0.25f;
        public float liftSpan = 5;

        [HideInInspector] public bool opening = false;
        [HideInInspector] public Color textColor;
        [HideInInspector] public Color textBoxColor;
        [HideInInspector] public RectTransform textBoxRect;
        [HideInInspector] public Vector2 currentSize;

        public void Initialize()
        {
            textBoxRect = textBox.GetComponent<RectTransform>();
            textBoxRect.sizeDelta = initialBoxSize;
            currentSize = textBoxRect.sizeDelta;
            opening = false;

            textColor = text.color;
            textColor.a = 0;
            text.color = textColor;
            textBoxColor = textBox.color;
            textBoxColor.a = 1;
            textBox.color = textBoxColor;
            text.color = new Vector4(0,1,0,0);
            textBox.gameObject.SetActive(false);
            text.gameObject.SetActive(false);

        }
    }

    public AnimationSettings animSettings = new AnimationSettings();
    public UISettings uiSettings = new UISettings();

    float lifeTimer = 0;

    private void Start()
    {
        animSettings.Initialize();
        uiSettings.Initialize();
    }

    public void StartOpen()
    {
        uiSettings.opening = true;
        uiSettings.textBox.gameObject.SetActive(true);
        uiSettings.text.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(uiSettings.opening)
        {
            OpenToolTip();

            if(animSettings.widthOpen && animSettings.heightOpen)
            {
                lifeTimer += Time.deltaTime;
                if (lifeTimer > uiSettings.liftSpan)
                {
                    FadeToolTipOut();
                }
                else { FadeTextIn(); }
            }
        }
    }

    void OpenToolTip()
    {
        switch(animSettings.openStyle)
        {
            case AnimationSettings.OpenStyle.HeightToWidth:
                OpenHeightToWidth();
                break;
            case AnimationSettings.OpenStyle.WidthToHeight:
                OpenWidthToHeight();
                break;
            case AnimationSettings.OpenStyle.HightAndWidth:
                OpenHeightAndWidth();
                break;
            default:
                Debug.LogError("No animation is set for the selected open style!");
                break;
        }

        uiSettings.textBoxRect.sizeDelta = uiSettings.currentSize;
    }

    void OpenWidthToHeight()
    {
        if (!animSettings.widthOpen)
        { 
            OpenWidth(); 
        }
        else
        {
            OpenHeight();
        }
    }

    void OpenHeightToWidth()
    {
        if (!animSettings.heightOpen)
        {
            OpenHeight();
        }
        else
        {
            OpenWidth();
        }
    }

    void OpenHeightAndWidth()
    {
        if (!animSettings.widthOpen)
        {
            OpenWidth();
        }
        if (!animSettings.heightOpen)
        {
            OpenHeight();
        }
    }

    void OpenWidth()
    {
        uiSettings.currentSize.x = Mathf.Lerp(uiSettings.currentSize.x, uiSettings.openedBoxSize.x,animSettings.widthSmooth * Time.deltaTime);

        if(Mathf.Abs(uiSettings.currentSize.x - uiSettings.openedBoxSize.x) < uiSettings.snapToSizeDistance) 
        {
            uiSettings.currentSize.x = uiSettings.openedBoxSize.x;
            animSettings.widthOpen = true;
        }
    }

    void OpenHeight()
    {
        uiSettings.currentSize.y = Mathf.Lerp(uiSettings.currentSize.y, uiSettings.openedBoxSize.y, animSettings.heightSmooth * Time.deltaTime);

        if (Mathf.Abs(uiSettings.currentSize.x - uiSettings.openedBoxSize.x) < uiSettings.snapToSizeDistance)
        {
            uiSettings.currentSize.y = uiSettings.openedBoxSize.y;
            animSettings.heightOpen = true;
        }
    }

    void FadeTextIn()
    {
        uiSettings.textColor.a = Mathf.Lerp(uiSettings.textColor.a,1,animSettings.textSmooth * Time.deltaTime);
        uiSettings.text.color = uiSettings.textColor;
    }

    void FadeToolTipOut()
    {
        uiSettings.textColor.a = Mathf.Lerp(uiSettings.textColor.a, 0, animSettings.textSmooth * Time.deltaTime);
        uiSettings.text.color = uiSettings.textColor;
        uiSettings.textBoxColor.a = Mathf.Lerp(uiSettings.textBoxColor.a, 0, animSettings.textSmooth * Time.deltaTime);
        uiSettings.textBox.color = uiSettings.textBoxColor;

        if(uiSettings.textBoxColor.a < 0.01f)
        {
            uiSettings.opening = false;
            animSettings.Initialize();
            uiSettings.Initialize();
            lifeTimer = 0;
        }
    }
}
