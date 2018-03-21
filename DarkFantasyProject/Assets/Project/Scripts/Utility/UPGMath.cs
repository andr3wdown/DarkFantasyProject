﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jopi.UPG
{
    public static class UPGMath
    {
        public static float BringToValue(float current, float target, float rate)
        {
            if(current > target)
            {
                current -= rate;
                if(current <= target)
                {
                    current = target;
                }
            }
            else if(current < target)
            {
                current += rate;
                if(current >= target)
                {
                    current = target;
                }
            }
            return current;
        }
        public static Vector3 SlowRigidbodyVelocity(Vector3 velocity, float rate)
        {
            float velMag = velocity.magnitude;
            velMag *= rate;
            if(velMag < 0.1f)
            {
                return Vector3.zero;
            }
            return velocity.normalized * velMag;
        }
        public static Vector3 SlowPlanarRigidbodyVelocity(Vector3 velocity, float rate)
        {
            Vector3 planarVelocity = new Vector3(velocity.x, 0, velocity.z);
            float velMag = planarVelocity.magnitude;
            velMag *= rate;
            if (velMag < 0.1f)
            {
                return Vector3.zero + new Vector3(0, velocity.y, 0);
            }
            return planarVelocity.normalized * velMag + new Vector3(0, velocity.y, 0);
        }
        public static Vector3 LimitRigidbodyVelocity(Vector3 velocity, float max)
        {
            if(velocity.magnitude > max)
            {
                velocity = velocity.normalized * max;
            }
            return velocity;
        }
    }

}

