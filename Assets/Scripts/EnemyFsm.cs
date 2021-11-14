using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class EnemyFsm : MonoBehaviour
    {
        private Vector2 moveDirection = Vector2.right;
        private Direction _direction = Direction.Right;

        private Movement2D _movement2D;

        private LayerMask tileLayer;
        private float rayDistance = 0.55f;

        private AroundWrap _aroundWrap;

        [SerializeField] private Sprite[] images;
        [SerializeField] private StageData stageData;
        [SerializeField] private float delayTime = 3.0f;
        private Direction _nextDirection = Direction.None;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            tileLayer = 1 << LayerMask.NameToLayer("Tile");
            _movement2D = GetComponent<Movement2D>();
            _aroundWrap = GetComponent<AroundWrap>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            SetMoveDirectionByRandom();
        }

        private void setMoveDirection(Direction direction)
        {
            _direction = direction;
            
            moveDirection = vector3FromEnum(_direction);
            _spriteRenderer.sprite = images[(int)_direction];
            
            StopAllCoroutines();
            StartCoroutine("SetMoveDirectionByTime");
        }

        private void SetMoveDirectionByRandom()
        {
            _direction = (Direction)Random.Range(0, (int)Direction.Count);
            setMoveDirection(_direction);
        }

        private IEnumerator SetMoveDirectionByTime()
        {
            yield return new WaitForSeconds(delayTime);

            int dir = Random.Range(0, 2);
            _nextDirection = (Direction)(dir * 2 + 1 - (int)_direction%2);
            StartCoroutine("CheckBlockedNextMoveDirection");
        }

        private IEnumerator CheckBlockedNextMoveDirection()
        {
            while (true)
            {
                Vector3[] directions = new Vector3[3];
                bool[]  isPossibleMove = new bool[3];

                directions[0] = vector3FromEnum(_nextDirection);
                if (directions[0].x != 0)
                {
                    directions[1] = directions[0] + new Vector3(0, 0.65f, 0);
                    directions[2] = directions[0] + new Vector3(0, -0.65f, 0);
                }
                else if (directions[0].y != 0)
                {
                    directions[1] = directions[0] + new Vector3( -0.65f, 0,0);
                    directions[2] = directions[0] + new Vector3( 0.65f, 0,0);
                }

                int possibleCount = 0;
                for (int i = 0; i < 3; ++i)
                {
                    if (i == 0)
                    {
                        isPossibleMove[i] = Physics2D.Raycast(transform.position, directions[i], 0.5f, tileLayer);
                        Debug.DrawLine(transform.position, transform.position + directions[i] * 0.5f, Color.yellow);
                        
                    }
                    else
                    {    isPossibleMove[i] = Physics2D.Raycast(transform.position, directions[i], 0.7f, tileLayer);
                        Debug.DrawLine(transform.position, transform.position + directions[i] * 0.7f, Color.yellow);

                        
                    }

                    if (isPossibleMove[i] == false)
                    {
                        possibleCount++;
                    }
                }

                if (possibleCount == 3)
                {
                    if (transform.position.x > stageData.LimitMin.x &&
                        transform.position.x < stageData.LimitMax.x &&
                        transform.position.y > stageData.LimitMin.y &&
                        transform.position.y < stageData.LimitMax.y
                    )
                    {
                        setMoveDirection(_nextDirection);
                        _nextDirection = Direction.None;
                        break;
                    }
                }

                yield return null;

            }
        }

        private Vector3 vector3FromEnum(Direction state)
        {
            Vector3 direction = Vector3.zero;
            switch(state)
            {
                case Direction.Up: direction = Vector3.up; break;
                case Direction.Left: direction = Vector3.left; break;
                case Direction.Right: direction = Vector3.right; break;
                case Direction.Down: direction = Vector3.down; break;
                    
            }

            return direction;
        }

        private void Update()
        {
          
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, rayDistance, tileLayer);
            if (hit.transform == null)
            {
                _movement2D.MoveTo(moveDirection);
                _aroundWrap.UpdateAroundWrap();
            }
            else
            {
              SetMoveDirectionByRandom();   
            }
        }
        
        
    }
}