using UnityEngine;

namespace Cali6 { 
    public static class A6_Help
    {
        public static void DebugPrint(bool debugLogsBool, string msgIn) { 
            if(debugLogsBool)
            {  Debug.Log(msgIn); }
        }
    }
}
