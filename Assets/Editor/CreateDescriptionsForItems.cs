using Arcatech;
using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Texts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateDescriptionsForItems : MonoBehaviour
{
    [MenuItem("Tools/Arcatech/Create and assign descriptions for weapon and skill configs")]
    private static void FindScriptableObjects()
    {
        var sk = Resources.FindObjectsOfTypeAll<SkillControlSettingsSO>();
        var items = Resources.FindObjectsOfTypeAll<Item>();
        string prefix = "desc_";
        string sk_prefix = "skill_";
        string i_prefix = "item_";

        foreach (var s in sk)
        {
            string descriptionFileTitle = prefix+sk_prefix+s.name+".asset";
            if (s.Description == null)
            {
                //if (!s.name.Contains(sk_prefix))
                //{
                //    s.name = sk_prefix + s.name;
                //}                
                ExtendedTextContainerSO asset = ScriptableObject.CreateInstance<ExtendedTextContainerSO>();
                string name = descriptionFileTitle;
                AssetDatabase.CreateAsset(asset, Constants.Texts.c_SkillsDesc+name);

                AssetDatabase.SaveAssets();
                s.Description = asset;
            }
        }

        foreach (var s in items)
        {
            string descriptionFileTitle = prefix + i_prefix + s.name + ".asset";
            if (s.Description == null)
            {
                //if (!s.name.Contains(i_prefix))
                //{
                //    s.name = i_prefix + s.name;
                //}
                ExtendedTextContainerSO asset = ScriptableObject.CreateInstance<ExtendedTextContainerSO>();
                string name = descriptionFileTitle;
                AssetDatabase.CreateAsset(asset, Constants.Texts.c_WeaponsDesc + name);

                AssetDatabase.SaveAssets();
                s.Description = asset;
            }
        }
    }





}
