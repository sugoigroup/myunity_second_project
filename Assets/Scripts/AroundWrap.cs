using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AroundWrap : MonoBehaviour
    {
        [SerializeField] private StageData _stageData;

        public void UpdateAroundWrap()
        {
            Vector3 position = transform.position;

            if (position.x < _stageData.LimitMin.x || position.x > _stageData.LimitMax.x)
            {
                position.x *= -1;
            }
            if (position.y < _stageData.LimitMin.y || position.y > _stageData.LimitMax.y)
            {
                position.y *= -1;
            }

            transform.position = position;
        }
    }
}