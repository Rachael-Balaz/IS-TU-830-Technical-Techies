using UnityEngine;
using RPG.Saving;


namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoint = 100f;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            //if health go below 0 it will take 0
            healthPoint = Mathf.Max(healthPoint - damage, 0);
            if (healthPoint == 0)
            {
                die();
            }
            print(healthPoint);
        }

        private void die()
        {
            if(isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        
        //These 2 methods will save and Load Character HP 
        //Save Character HP
        public object CaptureState()
        {
            return healthPoint;
        }

        //Will be call after the level loaded just after awake
        //Load Character HP from save file
        public void RestoreState(object state)
        {
            healthPoint = (float)state;

            if (healthPoint == 0)
            {
                die();
            }
        }
    }
}
