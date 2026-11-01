using System.Collections;
using UnityEngine;

public class Enemy01 : MonoBehaviour
{
   [SerializeField] private float _speed;
   private Transform[] _wayPoints;
   private int _wayPointCount;
   private int _currentIndex = 0;

   public void SetUp(Transform[] wayPoints)
   {
      _wayPointCount = wayPoints.Length;
      _wayPoints = new Transform[_wayPointCount];
      _wayPoints = wayPoints;

      transform.position = _wayPoints[_currentIndex].position;
      _currentIndex++;
      
      //이동
      StartCoroutine(Process());
   }

   private IEnumerator Process()
   {
      while (true)
      {
         yield return StartCoroutine(MoveToB(transform.position, _wayPoints[_currentIndex].position));

         if (_currentIndex < _wayPointCount - 1)
            _currentIndex++;
         else
             Destroy(gameObject);
      }
   }

   private IEnumerator MoveToB(Vector3 start, Vector3 end)
   {
      float percent = 0;
      float moveTime = Vector3.Distance(start, end) * _speed;

      while (percent < 1)
      {
         percent += Time.deltaTime / moveTime;
         transform.position = Vector3.Lerp(start, end, percent);
         yield return null;
      }
   }
}
