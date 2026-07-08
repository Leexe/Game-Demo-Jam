#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Micwu.Util.Attributes
{
    [CustomPropertyDrawer(typeof(DescAttribute))]
    public class DescDrawer : PropertyDrawer
    {
        public const string HasDescClass = "hasDesc";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var attr = (DescAttribute)attribute;
            var container = new VisualElement { style = { flexDirection = FlexDirection.Column } };

            Color labelColor = EditorGUIUtility.isProSkin
                ? new Color(0.5f, 0.5f, 0.5f)
                : new Color(0.4f, 0.4f, 0.4f);

            var desc = new Label(attr.Text)
            {
                style =
        {
            fontSize = 10,
            whiteSpace = WhiteSpace.Normal,
            color = new StyleColor(labelColor),
            marginLeft = 3,
            marginRight = Length.Percent(65),
            marginBottom = 5,
        }
            };

            container.Add(new PropertyField(property));
            container.Add(desc);
            return container;
        }
    }
}

#endif