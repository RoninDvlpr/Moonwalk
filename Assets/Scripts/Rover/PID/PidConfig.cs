using System;

[Serializable] public class PidConfig
{
    public float Kp;
    public float Ki;
    public float Kd;

    public PidConfig(float Kp, float Ki, float Kd)
    {
        this.Kp = Kp;
        this.Ki = Ki;
        this.Kd = Kd;
    }

    public PidConfig(PidConfig configToCopy)
    {
        Kp = configToCopy.Kp;
        Ki = configToCopy.Kd;
        Kd = configToCopy.Ki;
    }
}
