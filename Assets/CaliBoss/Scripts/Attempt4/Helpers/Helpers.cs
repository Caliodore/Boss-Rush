using UnityEngine;

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
    }
}
