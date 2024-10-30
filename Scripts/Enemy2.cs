using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 1.2f;
    [SerializeField] private Vector2 _player;
    private Vector2 _moveDirection;
    private GameManager _gameManager;
    private Animator _animator;
    [SerializeField] private Animator _animator_player;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        _animator = GetComponent<Animator>();

        _animator_player = player.GetComponent<Animator>();
        Debug.Log(_animator_player.name);
        _rb = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>().transform.position;
        _moveDirection = new Vector2(_player.x - transform.position.x, _player.y - transform.position.y).normalized;
        _animator.SetBool("isMoving", true);
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveDirection * _speed;
        if (Math.Abs(_player.x - transform.position.x) < 2)
        {
            _animator.SetBool("isAttack", true);
            _moveDirection = Vector2.zero;
        }
        else if (_moveDirection.x < 0)
        {
            transform.localScale = new Vector2(-5, 5);
        }
        else
        {
            transform.localScale = new Vector2(5, 5);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        AnimatorStateInfo currentState = _animator_player.GetCurrentAnimatorStateInfo(0);
        if (_animator_player.GetBool("isPunching") || _animator_player.GetBool("isKicking") ||
            _animator_player.GetBool("isDoubleKick") || _animator_player.GetBool("isDoublePunch"))
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _animator.SetBool("Alive", false);
            _rb.velocity = new Vector2(0, 0);
            Destroy(gameObject, 2);
        }
    }
}
