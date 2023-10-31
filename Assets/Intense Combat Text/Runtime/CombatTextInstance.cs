using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

namespace DevPlz.CombatText
{
	//NOTE: This should only ever be created or interacted with by CombatText.cs.
	//This formats and animates the individual CombatText prefabs.
	public class CombatTextInstance : MonoBehaviour
	{
		public CombatTextStyle Style { get; private set; }
		public Transform Parent { get; private set; }
		[SerializeField]
		private TextMeshProUGUI myText;
		private float _timer = 0;
		private float _xMovement;
		private Camera _mainCam;
		private Image _bgImage;
		private CanvasGroup _cg;
        private void Awake()
        {
			_mainCam = Camera.main;
			_bgImage = GetComponentInChildren<Image>(true);
			_cg = GetComponent<CanvasGroup>();
			gameObject.SetActive(false);
        }
		/// <summary>
		/// Initializes a new CombatTextInstance to be added to the pool.
		/// </summary>
		/// <param name="textStyle">The CombatTextStyle to apply to this instance</param>
		/// <param name="parent">The pool sub-object to child the new CombatTextInstance to</param>
        public void Initialize(CombatTextStyle textStyle, Transform parent)
		{
			Style = Instantiate(textStyle);
			Parent = parent;
			myText.color = Style.textGradient.Evaluate(0); ;
			myText.font = Style.font;
			myText.fontSize = Style.fontSize;
			if (Style.background) 
			{
				_bgImage.sprite = Style.background;
				_bgImage.type = Style.background.border == Vector4.zero ? Image.Type.Simple : Image.Type.Sliced;
				_bgImage.gameObject.SetActive(true);
			} else _bgImage.gameObject.SetActive(false);
		}

		/// <summary>
		/// Activates, formats, and positions a pooled CombatTextInstance.
		/// </summary>
		/// <param name="text">The string to be displayed by the CombatTextInstance</param>
		/// <param name="position">The location in World space to spawn the new CombatTextInstance</param>
		/// <param name="followTarget">OPTIONAL: The GameObject for the new the CombatTextInstance to follow. NOTE: It is recommended that the followed object have a CombatTextDetacher component.</param>
		public void Spawn(string text, Vector3 position, Transform followTarget = null) 
		{
			transform.SetParent(followTarget == null ? Parent : followTarget);
			myText.rectTransform.anchoredPosition = new Vector3(Style.xMovementCurve.Evaluate(0) * _xMovement,
										Style.yMovementCurve.Evaluate(0) * Style.yMovement, 0);
			myText.SetText(BuildString(text));
			transform.position = new Vector3(position.x, position.y, position.z);
			
			if (Style.xVariance) _xMovement = Random.Range(Style.xMovement * -1, Style.xMovement);
			else _xMovement = Style.xMovement;
			
			_timer = 0;
			gameObject.SetActive(true);
		}
        private void Update()
        {
			//Animates the text and controls despawn
			if(_timer >= Style.duration) Despawn();
			_timer += Time.deltaTime;
			float elapsed = _timer / Style.duration;
			Vector3 pos = new Vector3(Style.xMovementCurve.Evaluate(elapsed) * _xMovement,
										Style.yMovementCurve.Evaluate(elapsed) * Style.yMovement, 0);
			myText.rectTransform.localScale = Vector3.one * Style.scaleCurve.Evaluate(elapsed);
			_cg.alpha = Style.fadeCurve.Evaluate(elapsed);
			myText.rectTransform.anchoredPosition = pos;
			myText.color = Style.textGradient.Evaluate(elapsed);
		}
		void LateUpdate()
		{
			// Rotates the text to face the camera
			Vector3 targetPos = transform.position + _mainCam.transform.rotation * Vector3.forward;
			Vector3 targetOrientation = _mainCam.transform.rotation * Vector3.up;
			transform.LookAt(targetPos, targetOrientation);
		}
		private string BuildString(string message) 
		{
            //Using StringBuilder for performance optimization
            StringBuilder sb = new StringBuilder();
			sb.Append(Style.messagePrefix);
			sb.Append(message);
			sb.Append(Style.messageSuffix);
			sb.Replace("\\n", "\n");
			return sb.ToString();
		}
		public void Despawn() => gameObject.SetActive(false);
    }
}