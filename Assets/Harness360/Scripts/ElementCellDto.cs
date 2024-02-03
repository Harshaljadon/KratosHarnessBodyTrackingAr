namespace UnityEngine.UI.Extensions.Examples
{
    public enum ButtonType
    {
        harnessMenuButton,
        harnessVariation,
        colorMenuButton,
        colorVariation
    };
    
    public class ElementCellDto
    {
        public string Message;
        public Sprite icon;
        public ButtonType buttonType;
        public Element.ElementType elementType = Element.ElementType.Buckle;
    }
}
