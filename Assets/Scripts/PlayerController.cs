using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 moveDirection = Vector2.right;
        private Movement2D movement2D;
        private Direction _direction = Direction.Right;
        private AroundWrap _aroundWrap;
        private SpriteRenderer _spriteRenderer;
        
        private LayerMask tileLayer;
        private float rayDistance = 0.55f;

        private void Awake()
        {
            tileLayer = 1 << LayerMask.NameToLayer("Tile");
            
            movement2D = GetComponent<Movement2D>();
            _aroundWrap = GetComponent<AroundWrap>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown((KeyCode.UpArrow)))
            {
                moveDirection = Vector2.up;
                _direction = Direction.Up;
            }
            else if (Input.GetKeyDown((KeyCode.LeftArrow)))
            {
                moveDirection = Vector2.left;
                _direction = Direction.Left;
            }
            else if (Input.GetKeyDown((KeyCode.RightArrow)))
            {
                moveDirection = Vector2.right;
                _direction = Direction.Right;
            }
            else if (Input.GetKeyDown((KeyCode.DownArrow)))
            {
                moveDirection = Vector2.down;
                _direction = Direction.Down;
            }

            RaycastHit2D hit= Physics2D.Raycast(transform.position, moveDirection, rayDistance, tileLayer);
            if (hit.transform == null)
            {
                bool movePossible = movement2D.MoveTo(moveDirection);
                if (movePossible)
                {
                    transform.localEulerAngles = Vector3.forward * 90 * (int)_direction;
                }
                _aroundWrap.UpdateAroundWrap();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Item"))
            {
                Destroy(collision.gameObject);
            }
            if (collision.CompareTag("Enemy"))
            {
                StopCoroutine("OnHit");
                StartCoroutine("OnHit");
                Destroy(collision.gameObject);
            }
        }

        private IEnumerator OnHit()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = Color.white;
        }
    }
}