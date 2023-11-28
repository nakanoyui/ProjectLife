using System.Collections.Generic;
using UnityEngine;

public class ItemPresets : MonoBehaviour
{
    [SerializeField] private List<Item> _presets = new();

    public void ChangeItem(ref Item item, int index)
    {
        if (item == null)
        {
            item = _presets[index];
            item.gameObject.SetActive(true);
        }
        else
        {
            item.gameObject.SetActive(false);
            item = _presets[index];
            item.gameObject.SetActive(true);
        }
    }
}