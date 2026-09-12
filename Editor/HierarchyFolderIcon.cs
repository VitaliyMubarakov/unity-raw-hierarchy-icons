using System.Collections.Generic;
using Unity.Hierarchy;
using Unity.Hierarchy.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace I3sn.RawHierarchyIcons
{
    [InitializeOnLoad]
    internal static class HierarchyFolderIcon
    {
        internal const string ClosedIconName = "Folder Icon";
        const string OpenIconName = "FolderOpened Icon";
        const string DarkPrefix = "d_";

        static readonly Dictionary<HierarchyViewItem, EventCallback<ChangeEvent<bool>>> Hooks = new();

        static HierarchyFolderIcon()
        {
            HierarchyWindow.BindViewItem -= OnBindItem;
            HierarchyWindow.BindViewItem += OnBindItem;
            HierarchyWindow.UnbindViewItem -= OnUnbindItem;
            HierarchyWindow.UnbindViewItem += OnUnbindItem;
        }

        static void OnBindItem(HierarchyWindow window, HierarchyView view, HierarchyViewItem item)
        {
            Unhook(item);

            if (item.Handler is not HierarchyGameObjectHandler handler)
                return;

            GameObject go = handler.GetGameObject(item.Node);
            if (go == null)
                return;

            Texture2D custom = EditorGUIUtility.GetIconForObject(go);

            if (custom == null)
            {
                item.Icon.style.backgroundImage = StyleKeyword.Null;
                return;
            }

            if (!IsFolderIcon(custom))
                return;

            Apply(item, view.IsExpanded(item.Node));

            if (item.Toggle == null)
                return;

            void Hook(ChangeEvent<bool> evt) => Apply(item, evt.newValue);

            EventCallback<ChangeEvent<bool>> hook = Hook;
            item.Toggle.RegisterValueChangedCallback(hook);
            Hooks[item] = hook;
        }

        static void OnUnbindItem(HierarchyWindow window, HierarchyView view, HierarchyViewItem item) => Unhook(item);

        static bool IsFolderIcon(Texture2D icon) =>
            icon.name == ClosedIconName || icon.name == DarkPrefix + ClosedIconName;

        static void Unhook(HierarchyViewItem item)
        {
            if (Hooks.Remove(item, out EventCallback<ChangeEvent<bool>> hook) && item.Toggle != null)
                item.Toggle.UnregisterValueChangedCallback(hook);
        }

        static void Apply(HierarchyViewItem item, bool expanded)
        {
            string name = expanded ? OpenIconName : ClosedIconName;
            if (EditorGUIUtility.IconContent(name).image is not Texture2D icon)
                return;

            item.Icon.style.backgroundImage = new StyleBackground(icon);
        }
    }
}
