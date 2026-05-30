using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "List of skill upgradeData - ", menuName = "RPG Setup/Skill/Skill List")]
public class Skill_ListDataSO : ScriptableObject
{
    public Skill_BaseSO[] skillList;

    //public Skill_DataSO GetItemData(string saveId)
    //{
    //    return itemList.FirstOrDefault(item => item != null && item.saveId == saveId);
    //}

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all Skill_BaseSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:Skill_BaseSO");

        skillList = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<Skill_BaseSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
