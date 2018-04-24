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
        //cameraContainer = new GameObject("Camera Container");
        //cameraContainer.transform.position = transform.position;
        //transform.SetParent(cameraContainer.transform);
        //gyroEnabled = EnableGryo();

		gyro = Input.gyro;
		gyro.enabled = true;
    }

    /*private bool EnableGryo()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rotation = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }*/

    private void Update()
    {
        /*if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rotation;
        }*/
		if (GameStateController.Instance.GetGameState() == GameState_e.GAME) {
			Vector3 previousEulerAngles = transform.eulerAngles;
			Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;

			Vector3 targetEulerAngles = previousEulerAngles + gyroInput * (Time.deltaTime / 1.5f) * Mathf.Rad2Deg;
			targetEulerAngles.x = 0.0f;
			targetEulerAngles.z = 0.0f;


			if (targetEulerAngles.y < 180) {
				transform.eulerAngles = new targetEulerAngles;
				Mathf.Clamp (targetEulerAngles.y, 0f, 75f);
			} else {
				Mathf.Clamp (targetEulerAngles.y-360f, -65f, 0f);
			}

			transform.eulerAngles = targetEulerAngles;
		}
    }

}
