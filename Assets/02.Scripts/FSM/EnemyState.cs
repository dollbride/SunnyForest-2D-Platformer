using Platformer.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    private CapsuleCollider2D _enemyCol;
    private PlayerMachine _playerMachine;

    private void Awake()
    {
        _enemyCol = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_playerMachine.onHpChanged += DepleteHp(5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
