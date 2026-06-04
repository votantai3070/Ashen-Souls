using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "List of character data - ", menuName = "RPG Setup/Player Data/Character List")]
public class Character_ListDataSO : ScriptableObject
{
    public PlayerDataSO[] characterList;

    public PlayerDataSO GetCharacterData(string saveId)
    {
        return characterList.FirstOrDefault(item => item != null && item.characterId == saveId);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all Skill_BaseSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:PlayerDataSO");

        characterList = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<PlayerDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
