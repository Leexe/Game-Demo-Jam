#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Micwu.Util.Attributes
{
    [CustomPropertyDrawer(typeof(FindOnSelfAttribute))]
    public class FindOnSelfDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.PropertyField(position, property, label);
                Debug.LogWarning($"[FindOnSelf] '{property.name}' is not an Object reference field. Attribute ignored.");
                return;
            }

            MonoBehaviour owner = property.serializedObject.targetObject as MonoBehaviour;
            if (owner == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            System.Type fieldType = fieldInfo.FieldType;
            Component selfComponent = owner.GetComponent(fieldType);
            Component currentRef = property.objectReferenceValue as Component;

            // auto-resolve
            if (currentRef == null && selfComponent != null)
            {
                property.objectReferenceValue = selfComponent;
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                MarkDirty(owner);
                currentRef = selfComponent;
            }

            // display
            bool isMissing = currentRef == null;
            bool isOverride = !isMissing && currentRef != selfComponent;

            string statusTip;
            if (isMissing)
                statusTip = $"[FindOnSelf] No {fieldType.Name} found on this GameObject.";
            else if (isOverride)
                statusTip = $"[FindOnSelf] Override — manually assigned instead of auto-resolved from this GameObject.";
            else
                statusTip = $"[FindOnSelf] Auto-resolved {fieldType.Name} from this GameObject.";

            string existingTooltip = label.tooltip;
            string combinedTooltip = string.IsNullOrEmpty(existingTooltip)
                ? statusTip
                : $"{existingTooltip}\n\n{statusTip}";

            GUIContent labelWithTooltip = new(label.text, label.image, combinedTooltip);

            //

            Color prevBg = GUI.backgroundColor;

            if (isOverride)
                GUI.backgroundColor = Color.orange;
            else if (!isMissing)
                GUI.backgroundColor = Color.cyan;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, labelWithTooltip);

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                MarkDirty(owner);
            }

            GUI.backgroundColor = prevBg;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private static void MarkDirty(MonoBehaviour owner)
        {
            EditorUtility.SetDirty(owner);

            if (!Application.isPlaying)
            {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                    EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                else if (owner.gameObject.scene.IsValid())
                    EditorSceneManager.MarkSceneDirty(owner.gameObject.scene);
            }
        }
    }

    /// <summary>
    /// Global hook that resolves [FindOnSelf] fields even when the inspector
    /// isn't open. Fires on play-mode entry so you never hit play with null refs
    /// </summary>
    [InitializeOnLoad]
    public static class FindOnSelfGlobalResolver
    {
        static FindOnSelfGlobalResolver()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                ResolveAll();
        }

        private static void ResolveAll()
        {
            var allMonoBehaviours = Object.FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include, FindObjectsSortMode.None);

            int resolved = 0;

            foreach (var mb in allMonoBehaviours)
            {
                var fields = mb.GetType().GetFields(
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    if (field.GetCustomAttributes(typeof(FindOnSelfAttribute), true).Length == 0)
                        continue;

                    if (!typeof(Component).IsAssignableFrom(field.FieldType))
                        continue;

                    var current = field.GetValue(mb) as Component;
                    if (current != null) continue;

                    var found = mb.GetComponent(field.FieldType);
                    if (found != null)
                    {
                        field.SetValue(mb, found);
                        EditorUtility.SetDirty(mb);
                        resolved++;
                    }
                }
            }

            if (resolved > 0)
                Debug.Log($"[FindOnSelf] Auto-resolved {resolved} field(s).");
        }
    }
}

#endif