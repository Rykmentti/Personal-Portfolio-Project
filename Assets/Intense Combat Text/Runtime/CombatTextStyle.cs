using TMPro;
using UnityEngine;

namespace DevPlz.CombatText
{
	[CreateAssetMenu(fileName = "NewCombatTextStyle", menuName = "Intense Combat Text/New Combat Text Style", order = 25)]
	public class CombatTextStyle : ScriptableObject
	{
		[Tooltip("The time from spawn until the new text automatically despawns."), Header("General")]
		public float duration = 2f;
		[Tooltip("A random offset from the spawn location on the X axis. Example: Using an offset of 1 will offset it randomly between -1 and 1")]
		public float randomOffset = .5f;
		[Tooltip("Optional UI sprite to display behind the text. Supports regular and 9 sliced sprites.")]
		public Sprite background;
		[Tooltip("Optional text to always include before the main text. Can include Rich Text markup. (See 'Gold' and 'Absorb' styles for an example.)")]
		public string messagePrefix;
		[Tooltip("Optional text to always include after the main text. Can include Rich Text markup. (See 'Gold' and 'Absorb' styles for an example.)")]
		public string messageSuffix;

		[Tooltip("The TextMeshPro FontAsset to use for the text"), Header("Text Appearance")]
		public TMP_FontAsset font;
		[Tooltip("The size of the font to use for the text")]
		public float fontSize = 20f;
		[Tooltip("The color of the text. If a gradient is used, it will animate its color from left to right over the duration")]
		public Gradient textGradient;
		[Tooltip("Optional prefab variant to use instead of the default CombatTextInstance Prefab. Only needed if some of your styles need to be significantly different from others")]
		public GameObject textPrefabVariant;

		[Tooltip("Controls the text's y position over its duration, multiplied by the yMovementCurve"),Header("Animation")]
		public float yMovement;
		[Tooltip("The text's y movement curve over its duration, multiplied by the yMovement")]
		public AnimationCurve yMovementCurve;
		[Tooltip("Controls the text's x position over its duration, multiplied by the xMovementCurve")]
		public float xMovement;
		[Tooltip("The text's x movement curve over its duration, multiplied by the xMovement")]
		public AnimationCurve xMovementCurve;
		[Tooltip("When true, instead of xMovement being a static value, each newly spawned text's xMovement will choose a random value being xMovement and -xMovement.")]
		public bool xVariance;
		[Tooltip("Controls the alpha value of the text over its duration. 1 for full opacity, 0 for fully transparent.")]
		public AnimationCurve fadeCurve;
		[Tooltip("Controls the scale of the text over the duration.")]
		public AnimationCurve scaleCurve;
    }
}
