using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{ 
    [SerializeField] Transform _itemPoint;
    [SerializeField] List<GameObject> _items = new List<GameObject>();
    int _currentIem;
  
    void Start()
    {
        _currentIem = 0;
    }
    void Update()
    {  
        if(Input.GetButtonDown("SwitchWeaponUp"))
            ChangeInventory(1);
        else if(Input.GetButtonDown("SwitchWeaponDown"))
            ChangeInventory(-1);

    }

    void ChangeInventory(int i)
    { 
        if(i > 0) 
            _currentIem = _currentIem >= _items.Count - 1 ? 0 : ++_currentIem; 
        else 
            _currentIem = _currentIem == 0 ? _items.Count - 1: --_currentIem;

        SwitchWeapon();
            
    }

    void SwitchWeapon()
    { 
        for(int i = 0; i < _items.Count; i++)
            _items[i].SetActive(i == _currentIem);
    }
}
