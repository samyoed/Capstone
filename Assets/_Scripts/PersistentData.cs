using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ballGame
{
    public class PersistentData : MonoBehaviour
    {
        public static int team1Points;
        public static int team2Points;

        public static Vector3 lastSceneCameraPosition = new Vector3(0,15,-10);

    }
}
