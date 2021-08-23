using UnityEngine;

namespace Others.TimeDestroyer
{
    /// <summary>
    /// Сущность, которая через задержку уничтожает GameObject
    /// </summary>
    public class TimeDestroyer : MonoBehaviour
    {
        [Header("Время уничтожения"), SerializeField]
        private float _timeDestroy = 1;
        
        private void OnValidate ()
        {
            if (_timeDestroy <= 0)
            {
                _timeDestroy = 1;
            }
        }
        
        private void Start () => Destroy(gameObject, _timeDestroy);
    }
}
