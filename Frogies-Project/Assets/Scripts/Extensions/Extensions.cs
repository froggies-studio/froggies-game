using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static int LayerToIndex(this LayerMask layerMask)
        {
            if (layerMask.value == 0)   
            {
                throw new UnityException("LayerMask value is 0");
            }
            
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