using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DevPlz.CombatText
{
    public class Billboard : MonoBehaviour
    {
		private Camera mainCam;
        private void Awake()
        {
            mainCam = Camera.main;
        }
        void LateUpdate()
		{
			Vector3 targetPos = transform.position + mainCam.transform.rotation * Vector3.forward;
			Vector3 targetOrientation = mainCam.transform.rotation * Vector3.up;
			transform.LookAt(targetPos, targetOrientation);
		}
	}
}