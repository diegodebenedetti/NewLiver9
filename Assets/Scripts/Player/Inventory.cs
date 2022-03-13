using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Inventory : MonoBehaviour
{ 
    [SerializeField] Transform _itemPoint;
    [SerializeField] List<GameObject> _items = new List<GameObject>();
    int _currentIem;
  
    void Start()
    { 
        EnemyAI.OnStateChange += HandleEnemyStateChange;
        _currentIem = 0;
    }
    void Update()
    {  
        if(Input.GetButtonDown("SwitchWeaponUp"))
            ChangeInventory(1);
        else if(Input.GetButtonDown("SwitchWeaponDown"))
            ChangeInventory(-1);

    }

    void HandleEnemyStateChange(EnemyState state)
    {
        switch(state)
        {
            case EnemyState.Materialized: 
                if(_currentIem != 1)
                    ChangeInventory(1); 
                break;
            default: 
                if (_currentIem != 0)
                    ChangeInventory(0); 
                break;
        }
    }
    
    void ChangeInventory(int i)
    { 
        _itemPoint.GetComponent<Animator>().SetTrigger("SwitchWeapon"); 
        if(i > 0) 
            _currentIem = _currentIem >= _items.Count - 1 ? 0 : ++_currentIem; 
        else 
            _currentIem = _currentIem == 0 ? _items.Count - 1: --_currentIem;
    }

    void SwitchWeapon()
    {  
        for(int i = 0; i < _items.Count; i++)
            _items[i].SetActive(i == _currentIem);
    }
     
}
