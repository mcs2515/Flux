using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControls : MonoBehaviour {

    private bool gyroEnabled;
    private Gyroscope gyro;
    private GameObject cameraContainer;
    private Quaternion rotation;

    private void Start()
    {
		gyro = Input.gyro;
		gyro.enabled = true;
    }

    private void Update()
    {
		if (GameStateController.Instance.GetGameState() == GameState_e.GAME) {
			Vector3 previousEulerAngles = transform.eulerAngles;
			Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;

			Vector3 targetEulerAngles = previousEulerAngles + gyroInput * (Time.deltaTime / 1.5f) * Mathf.Rad2Deg;
			targetEulerAngles.x = 0.0f;
			targetEulerAngles.z = 0.0f;

			transform.eulerAngles = new Vector3(targetEulerAngles.x, ClampAngle(targetEulerAngles.y, -60f, 60f),  targetEulerAngles.z);
		}
    }

	//reference https://forum.unity.com/threads/limiting-rotation-with-mathf-clamp.171294/ 
	static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if(min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
 
        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }

}
