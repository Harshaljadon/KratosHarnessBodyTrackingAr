using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformSliders : MonoBehaviour
{
    public Transform target;
    private Vector3 defaultPosition;
    
    public RectTransform rootPanel;

    public RectTransform menuPanel;
    public RectTransform controlPanel;
    public RectTransform positionPanel;
    public RectTransform rotationPanel;
    public RectTransform scalePanel;

    public Toggle hideToggle;
    public RectTransform onPanel;
    public RectTransform offPanel;

    public ToggleGroup toggleGroup;
    public Toggle positionToggle;
    public Toggle rotationToggle;
    public Toggle scaleToggle;
    
    public Slider sliderPosX;
    public Slider sliderPosZ;

    public Slider sliderRotX;
    public Slider sliderRotY;
    public Slider sliderRotZ;

    public Slider sliderScale;

    public bool positionEnabled = true;
    public bool rotationEnabled = true;
    public bool scalingEnabled = true;


    private float previousRotXValue;
    private float previousRotYValue;
    private float previousRotZValue;
    
    private void Awake()
    {
        defaultPosition = target.position;
        positionToggle.isOn = false;
        rotationToggle.isOn = false;
        scaleToggle.isOn = false;

        previousRotXValue = sliderRotX.value;
        previousRotYValue = sliderRotY.value;
        previousRotZValue = sliderRotZ.value;

    }

    private void Start()
    {
        RefreshToggles();
        ToggleWholePanel();
    }

    public void EnableRootPanel()
    {
        rootPanel.gameObject.SetActive(true);
    }
    
    public void DisableRootPanel()
    {
        rootPanel.gameObject.SetActive(false);
    }

    public void ToggleWholePanel()
    {
        if (hideToggle.isOn)
        {
            onPanel.gameObject.SetActive(true);
            offPanel.gameObject.SetActive(false);
        }
        else
        {
            onPanel.gameObject.SetActive(false);
            offPanel.gameObject.SetActive(true);
        }
        
        menuPanel.gameObject.SetActive(hideToggle.isOn);
        controlPanel.gameObject.SetActive(hideToggle.isOn);
        
    }

    public void DisableWholePanel()
    {
        hideToggle.isOn = false;
        
        onPanel.gameObject.SetActive(true);
        offPanel.gameObject.SetActive(false);
        
        menuPanel.gameObject.SetActive(hideToggle.isOn);
        controlPanel.gameObject.SetActive(hideToggle.isOn);
        
    }
    
    public void RefreshToggles()
    {
        if (positionToggle.isOn)
        {
            positionPanel.gameObject.SetActive(positionEnabled);
            rotationPanel.gameObject.SetActive(false);
            scalePanel.gameObject.SetActive(false);
        }
        else if (rotationToggle.isOn)
        {
            positionPanel.gameObject.SetActive(false);
            rotationPanel.gameObject.SetActive(rotationEnabled);
            scalePanel.gameObject.SetActive(false);
        }
        else if (scaleToggle.isOn)
        {
            positionPanel.gameObject.SetActive(false);
            rotationPanel.gameObject.SetActive(false);
            scalePanel.gameObject.SetActive(scalingEnabled);
        }
        else
        {
            positionPanel.gameObject.SetActive(false);
            rotationPanel.gameObject.SetActive(false);
            scalePanel.gameObject.SetActive(false);
        }
        
    }
    
    public void OnSliderPosXChange(Slider slider)
    {
        target.position = new Vector3(defaultPosition.x + slider.value, target.position.y, target.position.z);
    }
    
    public void OnSliderPosZChange(Slider slider)
    {
        target.position = new Vector3(target.position.x, target.position.y, defaultPosition.z + slider.value);
    }
    
    public void OnSliderRotXChange(Slider slider)
    {
        float delta = slider.value - previousRotXValue;
        target.transform.Rotate (Vector3.right * delta * 360);
        previousRotXValue = slider.value;
    }
    
    public void OnSliderRotYChange(Slider slider)
    {
        float delta = slider.value - previousRotYValue;
        target.transform.Rotate (Vector3.up * delta * 360);
        previousRotYValue = slider.value;
    }
    
    public void OnSliderRotZChange(Slider slider)
    {
        float delta = slider.value - previousRotZValue;
        target.transform.Rotate (Vector3.forward * delta * 360);
        previousRotZValue = slider.value;
    }
    
    public void OnSliderScale(Slider slider)
    {
        target.localScale = new Vector3(slider.value, slider.value, slider.value);
    }

}
