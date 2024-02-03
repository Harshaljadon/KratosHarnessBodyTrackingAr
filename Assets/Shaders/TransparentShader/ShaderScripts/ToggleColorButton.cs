
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorButton : MonoBehaviour
{
    public Button[] _buttonsToggle;

    private Color normalColor = Color.white; // Default color of buttons
    private Color selectedColor = Color.green; // Color of buttons when selected

    public Button selectedButton;

    void Start()
    {
        foreach (var item in _buttonsToggle)
        {
            item.onClick.AddListener(() => { ToggleColor(item); });
        }
    }

    void ToggleColor(Button clickedButton)
    {
        selectedButton = clickedButton;


        foreach (var item in _buttonsToggle)
        {
            SetButtonColor(item, normalColor);
        }
        SetButtonColor(clickedButton, selectedColor);
    }

    void SetButtonColor(Button button, Color color)
    {
        var colors = button.colors;
        colors.selectedColor = color;
        button.colors = colors;
    }
}
