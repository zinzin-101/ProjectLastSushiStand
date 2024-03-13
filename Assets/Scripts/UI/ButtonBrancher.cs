using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonBrancher : MonoBehaviour
{
    public class ButtonScaler
    {
        enum ScaleMode { MatchWidthHeight, IndependentWidrhHeight}
        ScaleMode mode;
        Vector2 referenceButtonSize;

        [HideInInspector] 
        public Vector2 referenceScreenSize;
        public Vector2 newButtonSize;

        public void Initialize(Vector2 refButtonSize, Vector2 refScreenSize, int scaleMode)
        {
            mode = (ScaleMode)scaleMode;
            referenceButtonSize = refButtonSize;
            referenceScreenSize = refScreenSize;
            SetNewButtonSize();
        }

        void SetNewButtonSize()
        {
            if(mode == ScaleMode.IndependentWidrhHeight)
            {
                newButtonSize.x = (referenceButtonSize.x * Screen.width) / referenceScreenSize.x;
                newButtonSize.y = (referenceButtonSize.y * Screen.height) / referenceScreenSize.y;
            }else if(mode == ScaleMode.MatchWidthHeight)
            {
                newButtonSize.x = (referenceButtonSize.x * Screen.width) / referenceScreenSize.x;
                newButtonSize.y = newButtonSize.x;
            }
        }
    }

    [Serializable] public class RevealSettings
    {
        public enum RevealOption { Linear, Circular };
        public RevealOption option;
        public float translateSmooth = 5f;
        public float fadeSmooth = 0.01f;
        public bool revealOnStart = false;

        [HideInInspector] public bool opening = false;
        [HideInInspector] public bool spawned = false;

    }

    [Serializable] public class LinearSpawner
    {
        public enum RevealStyle { SlideToPosition, FadeInAtPosition };
        public RevealStyle revealStyle;
        public Vector2 direction = new Vector2(0, 1);
        public float baseButtonSpacing = 5f;
        public int buttonNumOffset = 0;

        [HideInInspector] public float buttonSpacing = 5f;

        public void FitSpacingToScreenSize(Vector2 refScreenSize)
        {
            float refScreenFloat = (refScreenSize.x + refScreenSize.y) / 2;
            float screenFloat = (Screen.width + Screen.height) / 2;
            buttonSpacing = (baseButtonSpacing + screenFloat) / refScreenFloat;
        }
    }

    [Serializable] public class CircularSpawner
    {
        public enum RevealStyle { SlideToPosition,FadeInAtPosition};
        public RevealStyle revealStyle;
        public Angle angle;
        public float baseDistFromBreacher = 20;

        [HideInInspector] public float distFromBreacher = 0;

        [Serializable] public struct Angle { public float minAngle; public float maxAngle; }

        public void FitDistanceToScreenSize(Vector2 refScreenSize)
        {
            float refScreenFloat = (refScreenSize.x + refScreenSize.y) / 2;
            float screenFloat = (Screen.width+Screen.height) / 2;
            distFromBreacher = (baseDistFromBreacher + screenFloat) / refScreenFloat;
        }
    }

    public GameObject[] buttonRefs;

    public enum ScaleMode { MatchWidthHeight, IndependentWidthHeight};
    public ScaleMode mode;
    public Vector2 referenceButtonSize;
    public Vector2 referenceScreenSize;

    ButtonScaler buttonScaler = new ButtonScaler();
    public RevealSettings revealSettings = new RevealSettings();
    public LinearSpawner linSpawner = new LinearSpawner();
    public CircularSpawner circSpawner = new CircularSpawner();
    [HideInInspector] public List<GameObject> buttons;

    float lastScreenWidth = 0;
    float lastScreenHeight = 0;

    void Start()
    {
        buttons = new List<GameObject>();
        buttonScaler = new ButtonScaler();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        buttonScaler.Initialize(referenceButtonSize, referenceScreenSize, (int)mode);
        circSpawner.FitDistanceToScreenSize(buttonScaler.referenceScreenSize);
        linSpawner.FitSpacingToScreenSize(buttonScaler.referenceScreenSize); 

        if(revealSettings.revealOnStart)
        {
            SpawnButtons();
        }
    }

    void Update()
    {
        if(Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            buttonScaler.Initialize(referenceButtonSize,referenceScreenSize, (int)mode);
            circSpawner.FitDistanceToScreenSize(buttonScaler.referenceScreenSize);
            linSpawner.FitSpacingToScreenSize(buttonScaler.referenceScreenSize);
            SpawnButtons();
        }
        if (revealSettings.opening)
        {
            if(!revealSettings.spawned)
            {
                SpawnButtons();
            }
            switch (revealSettings.option)
            {
                case RevealSettings.RevealOption.Linear:

                    switch (linSpawner.revealStyle)
                    {
                        case LinearSpawner.RevealStyle.SlideToPosition: RevealLinearlyNormal(); break;
                        //case LinearSpawner.RevealStyle.FadeInAtPosition: RevealLinearlyFade(); break;
                    }
                    break;
                case RevealSettings.RevealOption.Circular:
                    switch (circSpawner.revealStyle)
                    {
                        //case CircularSpawner.RevealStyle.SlideToPosition: RevealCirculatNormal(); break;
                        //case CircularSpawner.RevealStyle.FadeInAtPosition: RevealCirculatFade(); break;
                    }
                    break;
            }
        }

    }

    public void SpawnButtons()
    {
        revealSettings.opening = true;

        for(int i = buttons.Count - 1; i >= 0; i--)
        {
            Destroy(buttons[i]);
        }
        buttons.Clear();

        ClearCommonBottonBranchers();

        for(int i = 0; i < buttonRefs.Length; i++)
        {
            GameObject b = Instantiate(buttonRefs[i] as GameObject);
            b.transform.SetParent(transform);
            b.transform.position = transform.position;

            if (linSpawner.revealStyle == LinearSpawner.RevealStyle.FadeInAtPosition || circSpawner.revealStyle == CircularSpawner.RevealStyle.FadeInAtPosition)
            {
                Color c = b.GetComponent<UnityEngine.UI.Image>().color;
                c.a = 0;
                b.GetComponent<UnityEngine.UI.Image>().color = c;
                if (b.GetComponentInChildren<TMP_Text>())
                {
                    c = b.GetComponent<TMP_Text>().color;
                    c.a = 0;
                    b.GetComponentInChildren<TMP_Text>().color = c;
                }

            }
            buttons.Add(b);
        }
        revealSettings.spawned = true;
    }

    void RevealLinearlyNormal()
    {
        for(int i = 0;i<buttons.Count;i++)
        {
            Vector3 targetPos;
            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();

            buttonRect.sizeDelta = new Vector2(buttonScaler.newButtonSize.x, buttonScaler.newButtonSize.y);

            targetPos.x = linSpawner.direction.x * ((i + linSpawner.buttonNumOffset) * (buttonRect.sizeDelta.x + linSpawner.buttonSpacing) ) + transform.position.x;
            targetPos.y = linSpawner.direction.y * ((i + linSpawner.buttonNumOffset) * (buttonRect.sizeDelta.y + linSpawner.buttonSpacing) ) + transform.position.y;
            targetPos.z = 0;

            buttonRect.position = Vector3.Lerp(buttonRect.position, targetPos, revealSettings.translateSmooth * Time.deltaTime);
        }
    }
    void ClearCommonBottonBranchers()
    {
        GameObject[] branchers = GameObject.FindGameObjectsWithTag("ButtonBrancher");
        foreach (GameObject brancher in branchers) {
            if (brancher.transform.parent == transform.parent)
            {
                 ButtonBrancher bb = brancher.GetComponent<ButtonBrancher>();
                for(int i = bb.buttons.Count - 1; i >= 0; i--)
                {
                    Destroy(bb.buttons[i]);
                }
                bb.buttons.Clear();
            }
        }
    }

}

