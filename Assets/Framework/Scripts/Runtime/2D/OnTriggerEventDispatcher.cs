using UnityEngine;
using UnityEngine.Events;

namespace Cofdream.TwoD
{
    public class OnTriggerEventDispatcher : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Collider2D> triggerEnterEvent2D;
        public UnityEvent<Collider2D> TriggerEnterEvent2D => triggerEnterEvent2D;



        private void OnTriggerEnter2D(Collider2D collision)
        {
            triggerEnterEvent2D.Invoke(collision); 
        }
    }
}
