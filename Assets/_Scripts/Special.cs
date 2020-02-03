using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class Special : MonoBehaviour
    {
        public enum special{Grapple, Shoot, DashAttack};

        public special thisSpecial;


        // Update is called once per frame
        public void activateSpecial()
        {
            switch(thisSpecial)
            {
                case special.Grapple:
                    grapple();
                break;
                case special.Shoot:
                    shoot();
                break;
                case special.DashAttack:
                    moveAttack();
                break;

                default:
                break;
            }
        }

        void grapple()
        {

        }

        void shoot()
        {

        }

        void moveAttack()
        {
            
        }

    }
}
