using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GridSnap
    
    {
    /// <summary>
    /// Rounds a Vector3 to given factor while setting y to 0 (pivot will align with grid) 
    /// </summary>
    /// <param name="input">your object position</param>
    /// <param name="factor">determines the value 'input' will be rounded to (cannot be set to 0, value will be altered if sent)</param>
    /// <returns></returns>
   public static Vector3 SnapPosition(Vector3 input, float factor = 1f)
    {
        if (factor <= 0f)
        {

            factor = 1;
        }
    

       var RoundedVector = Round(input, factor);


        return RoundedVector;
        
    }
    
    public static Vector3 Round(Vector3 input, float factor) => (new Vector3(Mathf.Round(input.x / factor) * factor, 0, Mathf.Round(input.z / factor) * factor));


    /// <summary>
    /// Performs same rounding to transform as SnapPosition() but does not ground the Y value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    public static Vector3 FreeSnapPosition<T>(Vector3 input, float factor = 1f)
    {
        if (factor <= 0f)
            throw new UnityException("factor argument must be above 0");

        float x = Mathf.Round(input.x / factor) * factor;
        float y = Mathf.Round(input.y / factor) * factor;
        float z = Mathf.Round(input.z / factor) * factor;
      
        return new Vector3(x, y, z);
    }












}
