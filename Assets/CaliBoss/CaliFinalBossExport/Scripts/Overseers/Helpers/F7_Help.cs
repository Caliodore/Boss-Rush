using UnityEngine;

namespace Cali7 { 
    public static class F7_Help
    {
        public static void DebugPrint(bool debugLogsBool, string msgIn) { 
            if(debugLogsBool)
            {  Debug.Log(msgIn); }
        }
    }
}
