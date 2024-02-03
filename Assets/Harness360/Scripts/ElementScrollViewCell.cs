using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI.Extensions.Examples
{
    public class ElementScrollViewCell : FancyScrollViewCell<ElementCellDto, ElementScrollViewContext>
    {
        [SerializeField]
        Animator animator = null;
        [SerializeField]
        Text message = null;
        [SerializeField]
        Image image = null;
        [SerializeField]
        Button button = null;

        public Color32 normalColor;
        public Color32 selectionColor;
        
        private Element.ElementType _elementType;

        private ButtonType _buttonType;
        private UiManager _uiManager;
        
        static readonly int scrollTriggerHash = Animator.StringToHash("scroll");

        void Start()
        {
            _uiManager = FindObjectOfType<UiManager>().GetComponent<UiManager>();
            button.onClick.AddListener(OnPressedCell);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="itemData">Item data.</param>
        public override void UpdateContent(ElementCellDto itemData)
        {
            message.text = itemData.Message;
            _elementType = itemData.elementType;
            _buttonType = itemData.buttonType;
            image.sprite = itemData.icon;
            image.useSpriteMesh = true;

            if (Context != null)
            {
                var isSelected = Context.SelectedIndex == DataIndex;
                image.color = isSelected
                    ? selectionColor
                    : normalColor;
            }
        }

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position">Position.</param>
        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            animator.Play(scrollTriggerHash, -1, position);
            animator.speed = 0;
        }

        void OnPressedCell()
        {
            Context?.OnPressedCell(this);

            switch (_buttonType)
            {
                case ButtonType.harnessMenuButton:
                {
                    _uiManager.OnSelectHarnessButton();
                }
                    break;
                case ButtonType.harnessVariation:
                {
                    _uiManager.OnHarnessPressed(Context.SelectedIndex);
                    //_uiManager.OnMeshVariationPressed(_elementType, Context.SelectedIndex);
                }
                    break;
                case ButtonType.colorMenuButton:
                {
                    _uiManager.OnColorButtonPressed(_elementType);
                }
                    break;
                case ButtonType.colorVariation:
                {
                    _uiManager.OnColorVariationPressed(_elementType, Context.SelectedIndex);
                }
                    break;
                default:
                    break;
            }
            
            
        }

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;
        void OnEnable()
        {
            UpdatePosition(currentPosition);
        }
    }
}
