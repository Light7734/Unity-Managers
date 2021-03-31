using UnityEngine;

[System.Serializable]
public struct RangedFloat
{
    public float min;
    public float max;

    public RangedFloat(float min = 1f, float max = 1f)
        {
            this.min = min;
            this.max = max;
        }

        public float GetRandomInRange() { return min == max ? min : Random.Range(min, max); }
    }

    public class RangedFloatRangeAttribute : PropertyAttribute
    {
        public float min, max;

    public RangedFloatRangeAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}