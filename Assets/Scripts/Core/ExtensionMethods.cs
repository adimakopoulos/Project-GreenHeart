using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods 
{

    /// <summary>
    /// Rounds Vector3.
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1f;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }



    public static IEnumerable<T> RepopulateEXT<T>(this IEnumerable<T> input)
    {
        var repopulated = input.OrderBy(x => x);

        T prev;
        foreach (var element in input)
        {
            if (element != null)
            {
                yield return element;
            }


            
            prev = element;
        }
        //input.ToList<T>().TrimExcess();

    }
}
