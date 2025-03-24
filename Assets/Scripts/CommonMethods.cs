using System;

public static class CommonMethods
{
    public static float Remap(this float value, float oMin, float oMax, float nMin, float nMax)
    {
        float result = nMin + (value - oMin) * (nMax - nMin) / (oMax - oMin);
        return result;
    }

    public static float PreSpawnCalculation(float speed, float lineLength)
    {
        return lineLength / ((1 / UnityEngine.Time.deltaTime) * (speed * UnityEngine.Time.deltaTime));
    }

    public static float DistanceByTime(double firstTime, double secondTime, float speed)
    {
        double distance = ((secondTime - firstTime) * ((1 / UnityEngine.Time.deltaTime) * (speed * UnityEngine.Time.deltaTime)));
        UnityEngine.Debug.Log("Distance Answer: " + distance + " | Time Different: " + (secondTime - firstTime));
        return (float)distance;
    }
}
