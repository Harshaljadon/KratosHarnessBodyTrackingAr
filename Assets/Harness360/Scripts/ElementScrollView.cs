using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.Extensions.Examples
{
    public class ElementScrollView : FancyScrollView<ElementCellDto, ElementScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController = null;

        void Awake()
        {
            scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));
            SetContext(new ElementScrollViewContext { OnPressedCell = OnPressedCell });
        }

        public void UpdateData(List<ElementCellDto> data)
        {
            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }

        void OnPressedCell(ElementScrollViewCell cell)
        {
            scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
            Context.SelectedIndex = cell.DataIndex;
            UpdateContents();
        }
    }
}
