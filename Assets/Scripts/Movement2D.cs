using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField] private float moveTime = 0.2f;

    private bool isMove = false;

    public bool MoveTo(Vector3 moveDirection)
    {
        if (isMove)
        {
            return false;
        }

        StartCoroutine(SmoothGridMovement(transform.position + moveDirection));

        return true;
    }

    private IEnumerator SmoothGridMovement(Vector2 endPosition)
    {
        Vector2 startPosition = transform.position;
        float percent = 0;

        isMove = true;
        while (percent < moveTime)
        {
            percent += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, endPosition, percent / moveTime);

            yield return null;
        }

        isMove = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
