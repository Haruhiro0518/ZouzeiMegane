using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public enum ItemType
{
    People,
    Ishiba,
    // NewItem 
}

[CreateAssetMenu(menuName = "ScriptableObj/ItemLibrary")]
public class ItemLibrary : ScriptableObject
{
	[System.Serializable]
    public struct ItemEntry
    {
        public ItemType type;
        public GameObject prefab;
    }

	[SerializeField] private List<ItemEntry> itemEntries;
	private Dictionary<ItemType, GameObject> _cachedItems;

	// 辞書を初期化する（最初の一回だけ実行）
    private void InitializeCache()
    {
        if (_cachedItems != null) return;

        _cachedItems = new Dictionary<ItemType, GameObject>();
        foreach (var entry in itemEntries)
        {
            if (!_cachedItems.ContainsKey(entry.type))
            {
                _cachedItems.Add(entry.type, entry.prefab);
            }
        }
    }

    public GameObject GetPrefab(ItemType type)
    {
        InitializeCache();
        
        if (_cachedItems.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }
        
        Debug.LogError($"{type} に対応するPrefabが登録されていません！");
        return null;
    }

	public GameObject SelectItem(ValueData data)
	{
		float ishibaChance = data.IshibaSpawnChance;

		if(Random.value < ishibaChance)
		{
			return GetPrefab(ItemType.Ishiba);
		} else
		{
			return GetPrefab(ItemType.People);
		}
	}
}
