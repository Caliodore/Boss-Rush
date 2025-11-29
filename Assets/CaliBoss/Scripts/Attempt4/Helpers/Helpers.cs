using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cali_4
{
    public static class Helpers
    {
        public static void DebugPrint(string messageIn, bool testingEnabled)
        { 
            if(testingEnabled)
                MonoBehaviour.print(messageIn);
            else
                return;
        }

        //public static T GetRefFromII<T>(T refIn) where T : Component
        //{ 
        //    Type typeTarget = typeof(T);
        //    //C4_InspectorInterface.Instance
        //}
    }
}
