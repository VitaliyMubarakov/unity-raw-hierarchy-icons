using Unity.Hierarchy;
using Unity.Hierarchy.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace I3sn.RawHierarchyIcons
{
    [InitializeOnLoad]
    internal static class HierarchyIconContextMenu
    {
        const string Root = "Select Icon/";

        static readonly (string Label, string IconName)[] Builtin =
        {
            ("Folder", HierarchyFolderIcon.ClosedIconName),
            ("Component", "GridLayoutGroup Icon"),
            ("Player", "UnityEditor.GameView"),
        };

        static HierarchyIconContextMenu()
        {
            HierarchyWindow.PopulateContextMenu -= OnPopulate;
            HierarchyWindow.PopulateContextMenu += OnPopulate;
        }

        static void OnPopulate(HierarchyWindow window, HierarchyView view, HierarchyViewItem item, DropdownMenu menu)
        {
            if (item == null || item.Handler is not HierarchyGameObjectHandler || Selection.gameObjects.Length == 0)
                return;

            int index = 0;

            foreach ((string label, string iconName) in Builtin)
            {
                if (EditorGUIUtility.IconContent(iconName).image is not Texture2D icon)
                    continue;

                menu.InsertAction(index++, Root + label, _ => Apply(icon));
            }

            menu.InsertSeparator(Root, index++);
            menu.InsertAction(index, Root + "Clear Icon", _ => Apply(null));
        }

        static void Apply(Texture2D icon)
        {
            GameObject[] targets = Selection.gameObjects;
            if (targets.Length == 0)
                return;

            foreach (GameObject go in targets)
            {
                EditorGUIUtility.SetIconForObject(go, icon);
                EditorUtility.SetDirty(go);

                if (EditorUtility.IsPersistent(go))
                    AssetDatabase.SaveAssetIfDirty(go);
                else if (go.scene.IsValid())
                    EditorSceneManager.MarkSceneDirty(go.scene);
            }

            EditorApplication.RepaintHierarchyWindow();
        }
    }
}
