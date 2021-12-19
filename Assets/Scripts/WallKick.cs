using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallKick 
{
   public static string[] Test(string tag, int currentState, int nextState)
    {
        if (tag == "Ishape")
        {
            if(currentState == 0 && nextState == 1)
            {
                return new string[] { "-2,0", "1,0", "-2,-1", "1,2" };
            }
            else if(currentState == 1 && nextState == 0)
            {
                return new string[] { "2,0", "-1,0", "2,1", "-1,-2" };
            }
            else if(currentState == 1 && nextState == 2)
            {
                return new string[] { "-1,0", "2,0", "-1,2", "2,-1" };
            }
            else if(currentState == 2 && nextState == 1)
            {
                return new string[] { "1,0", "-2,0", "1,-2", "-2,1" };
            }
            else if (currentState == 2 && nextState == 3)
            {
                return new string[] { "2,0", "-1,0", "2,1", "-1,-2" };
            }
            else if (currentState == 3 && nextState == 2)
            {
                return new string[] { "-2,0", "1,0", "-2,-1", "1,2" };
            }
            else if (currentState == 3 && nextState == 0)
            {
                return new string[] { "1,0", "-2,0", "1,-2", "-2,1" };
            }
            else // 0 >> 3
            {
                return new string[] { "-1,0", "2,0", "-1,2", "2,-1" };
            }
        }

        if (tag == "Ashapes")
        {
            if(currentState == 0 && nextState == 1)
            {
                return new string[] { "-1,0", "-1,1", "0,-2", "-1,-2" };
            }
            else if(currentState == 1 && nextState == 0)
            {
                return new string[] { "1,0", "1,-1", "0,2", "1,2" };
            }
            else if(currentState == 1 && nextState == 2)
            {
                return new string[] { "1,0", "1,-1", "0,2", "1,2" };
            }
            else if(currentState == 2 && nextState == 1)
            {
                return new string[] { "-1,0", "-1,1", "0,-2", "-1,-2" };
            }
            else if (currentState == 2 && nextState == 3)
            {
                return new string[] { "1,0", "1,1", "0,-2", "1,-2" };
            }
            else if (currentState == 3 && nextState == 2)
            {
                return new string[] { "-1,0", "-1,-1", "0,2", "-1,2" };
            }
            else if (currentState == 3 && nextState == 0)
            {
                return new string[] { "-1,0", "-1,-1", "0,2", "-1,2" };
            }
            else // 0 >> 3
            {
                return new string[] { "1,0", "1,1", "0,-2", "1,-2" };
            }
        }

        return null;
    } // Test
}
