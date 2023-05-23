﻿using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static int LayerToIndex(this LayerMask layerMask)
        {
            int mask = layerMask.value;
            int layerNumber = 0;
        
            // Find the rightmost set bit in the binary representation
            while ((mask & 1) == 0)
            {
                mask >>= 1;
                layerNumber++;
            }
        
            return layerNumber;
        }
    }
}