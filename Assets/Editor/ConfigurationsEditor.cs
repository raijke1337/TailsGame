using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;

public class ConfigurationsEditor : EditorWindow
{
    [MenuItem("Window/UI Toolkit/ConfigurationsEditor")]
    public static void ShowExample()
    {
        ConfigurationsEditor wnd = GetWindow<ConfigurationsEditor>();
        wnd.titleContent = new GUIContent("Scriptable objects configurator");
    }

    private VisualElement right;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement root = rootVisualElement;

        // load configs
        var configs = Extensions.GetAssetsFromPath<ScriptableObject>(Constants.Configs.c_AllConfigsPath, true);

        // this makes a split view
        var split = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        root.Add(split);

        // set up the left part , this is referred to as "optimal"
        // here we have a list of found configs
        var left = new ListView();
        split.Add(left);

        left.makeItem = () => new Label();
        left.bindItem = (item, index) => { (item as Label).text = configs[index].name; };
        left.itemsSource = configs;

        // make a callback
        left.onSelectionChange += Left_onSelectionChange;

        // set up the right part
        right = new VisualElement();
        split.Add(right);
        right.Add(new Label { text = "test" });

    }

    private void Left_onSelectionChange(IEnumerable<object> items)
    {
        right.Clear();
        var selectedCfg = items.First() as ScriptableObject;
    }
}


