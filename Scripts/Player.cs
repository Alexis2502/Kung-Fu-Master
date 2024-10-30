using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _hp;
    private Rigidbody2D _rb;
    private bool isMoving;
    private bool isJumping;
    private bool isCrouching;
    private bool isKicking;
    private bool isPunching;
    private bool isDoubleKick;
    private bool isDoublePunch;
    private bool moveBoolLeft;
    private bool moveBoolRight;
    private bool isDead;
    private bool killedEnemy = false;

    private Animator _animator;

    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 1.0f;
    private float lastPunch = 0;
    private float deadTime = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("isJumping", isJumping);
        _animator.SetBool("isCrouching", isCrouching);
        _animator.SetBool("isKicking", isKicking);
        _animator.SetBool("isDoubleKick", isDoubleKick);
        _animator.SetBool("isDoublePunch", isDoublePunch);
        _animator.SetBool("isPunching", isPunching);
        _animator.SetBool("isDead", isDead);
    }

    private void FixedUpdate()
    {
        if (_rb != null)
        {
            if (isMoving)
            {
                if (moveBoolRight)
                {
                    isJumping = false;
                    _rb.transform.position = new Vector2(transform.position.x + _moveSpeed, transform.position.y);
                    _animator.SetFloat("moveX", 1);
                    transform.localScale = new Vector2(-5, 5);
                    isMoving = true;
                }
                else if (moveBoolLeft)
                {
                    isJumping = false;
                    transform.localScale = new Vector2(5, 5);
                    _rb.transform.position = new Vector2(transform.position.x - _moveSpeed, transform.position.y);
                    _animator.SetFloat("moveX", -1);
                    isMoving = true;
                }
            } else if (isDead && Time.time - deadTime >=0.5f)
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                isMoving = false;
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    isJumping=false;
                    isKicking = false;
                    isPunching = false;
                    isDoubleKick = false;
                    isDoublePunch = false;
                }
            }
         }
    }

    public void OnPointerDownLeft()
    {
        moveBoolLeft = true;
        isMoving = true;
    }

    public void OnPointerDownRight()
    {
        moveBoolRight = true;
        isMoving = true;
    }
    public void OnPointerDownDown()
    {
        _rb.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
        isCrouching = true;
    }

    public void OnPointerUp()
    {
        moveBoolLeft = false;
        moveBoolRight = false;
        isMoving = false;
    }

    public void OnPointerUpCrouch()
    {
        isCrouching = false;
        _rb.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
    }

    public void OnUpClick()
    {
        isJumping = true;
        isMoving=false;
    }

    public void OnClickKick()
    {
        clicked++;
        if (clicked == 1) {
            clicktime = Time.time;
            isKicking = true;
            isDoubleKick = false;
        }

        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            isKicking = false;
            isDoubleKick = true;

        }
        else if (clicked > 2 || Time.time - clicktime > 1) {
            clicked = 0;
            clicktime = 0;
            isKicking = true;
        }
    }

    public void OnClickPunch()
    {
        clicked++;
        if (clicked == 1)
        {
            clicktime = Time.time;
            isPunching = true;
            isDoublePunch = false;
        }

        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            isPunching = false;
            isDoublePunch = true;

        }
        else if (clicked > 2 || Time.time - clicktime > 1)
        {
            clicked = 0;
            clicktime = 0;
            isPunching = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Sylvia")
        {
            SceneManager.LoadScene("End");
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (isDead)
        {
            if (Time.time - deadTime >= 0.5f)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
        else
        {
            foreach (GameObject enemy in enemies){
                Animator enemyAnimator = enemy.GetComponent<Animator>();
                if ((enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))) && Time.time - lastPunch > 1)
                {
                    _hp -= 200;
                    lastPunch = Time.time;
                    killedEnemy = true;
                    Debug.Log(_hp);
                } else if (_hp <= 0)
                {
                    isDead = true;
                    deadTime = Time.time;
                }
                _rb.velocity = Vector2.zero;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            SceneManager.LoadScene("Floor2");
        }
    }
}
