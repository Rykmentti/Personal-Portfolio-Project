using System.Collections.Generic;
using UnityEngine;

namespace DevPlz.CombatText
{
    public class CombatText : MonoBehaviour
    {
        [SerializeField, Tooltip("Shows or hides all CombatText. Use the ShowText(false) method to despawn all active text.")]
        private bool _showText = true;
        [SerializeField, Tooltip("If True, this GameObject will persist between scenes.")]
        private bool _dontDestroyOnSceneChange = false;
        [SerializeField, Tooltip("Path to load CombatTextStyle assets from. Reset this component to set to default path. Must be contained in a 'Resources' folder.")]
        private string styleAssetPath = "CombatTextStyles/";
        [SerializeField] private GameObject _textPrefab;

        private static CombatText _i;
        private static Dictionary<TextStyle, CombatTextStyle> _textStyleDict = new Dictionary<TextStyle, CombatTextStyle>();
        private Dictionary<CombatTextStyle, CombatTextPool> _poolDict = new Dictionary<CombatTextStyle, CombatTextPool>();
        void Awake()
        {
            if (_i)
            {
                Destroy(gameObject);
                return;
            }
            if (!_textPrefab) _textPrefab = Resources.Load<GameObject>("Prefabs/CombatTextInstance");
            if (_dontDestroyOnSceneChange) DontDestroyOnLoad(gameObject);
            _i = this;
            //Loads all CombatTextStyles at assetPath, and associates them to a TextStyle value.
            //Use Tools/Combat Text/Refresh TextStyle enum to rebuild the enum values.
            if (_textStyleDict.Count > 0) return;
            System.Collections.IList list = System.Enum.GetValues(typeof(TextStyle));
            for (int i = 0; i < list.Count; i++)
            {
                TextStyle textStyle = (TextStyle)list[i];
                _textStyleDict.Add(textStyle, Resources.Load<CombatTextStyle>(styleAssetPath + textStyle.ToString()));
            }
        }
        /// <summary>
        /// Display a new CombatText instance with the chosen string & style at a point in the world with an optional follow target.
        /// </summary>
        /// <param name="style">The CombatTextStyle to apply to this instance</param>
        /// <param name="text">The string to be displayed by the CombatTextInstance</param>
        /// <param name="position">The location in World space to spawn the new CombatTextInstance</param>
        /// <param name="followTarget">OPTIONAL: The GameObject for the new the CombatTextInstance to follow. NOTE: It is recommended that the followed object have a CombatTextDetacher component.</param>
        public static void Spawn(CombatTextStyle style, string text, Vector3 position, Transform followTarget = null)
        {
            if (!_i._showText) return;
            _i.GetTextInstanceFromPool(style).Spawn(text, position, followTarget == null ? null : followTarget);
        }
        /// Display a new CombatText instance with the chosen string & style at a point in the world with an optional follow target.
        /// </summary>
        /// <param name="style">The CombatTextStyle to apply to this instance</param>
        /// <param name="text">The string to be displayed by the CombatTextInstance</param>
        /// <param name="position">The location in World space to spawn the new CombatTextInstance</param>
        /// <param name="followTarget">OPTIONAL: The GameObject for the new the CombatTextInstance to follow. NOTE: It is recommended that the followed object have a CombatTextDetacher component.</param>
        public static void Spawn(TextStyle type, string text, Vector3 position, Transform followTarget = null)
        {
            if (!_i._showText) return;
            CombatTextInstance instance = _i.GetTextInstanceFromPool(_textStyleDict[type]);
            instance.Spawn(text, position, followTarget == null ? null : followTarget);
        }
        //Return the next available CombatTextInstance from its pool. If none are available, instantiate a new one.
        private CombatTextInstance GetTextInstanceFromPool(CombatTextStyle style)
        {
            
            if (!_poolDict.ContainsKey(style)) InitStylePool(style);
            CombatTextPool pool = _poolDict[style];
            pool.index++;
            if (pool.index >= pool.list.Count - 1) pool.index = 0;
            if (pool.list.Count > 0 && !pool.list[pool.index]) pool.list[pool.index] = NewText(style, pool);
            if (pool.list.Count == 0 || pool.list[pool.index].isActiveAndEnabled)
            {
                pool.list.Insert(pool.index, NewText(style, pool));
            }
            return pool.list[pool.index];
        }
        //Instantiate & initialize a new inactive CombatTextInstance and add it to its appropriate pool.
        private CombatTextInstance NewText(CombatTextStyle style, CombatTextPool pool)
        {
            GameObject combatText = Instantiate(style.textPrefabVariant == null ? _i._textPrefab : style.textPrefabVariant, Vector3.zero, Quaternion.identity, pool.parent);
            CombatTextInstance instance = combatText.GetComponent<CombatTextInstance>();
            instance.Initialize(style, pool.parent);
            return instance;
        }
        //Initialize a new pool based on the chosen style
        private void InitStylePool(CombatTextStyle style)
        {
            CombatTextPool pool = new CombatTextPool(style);
            pool.parent.SetParent(_i.transform);
            _poolDict.Add(style, pool);
        }
        /// <summary>
        /// Can be called from the OnDestroy/OnDisable method of a follow target to detach any CombatTextInstances that are attached to it. Alternatively a CombatTextDetacher component can do this automatically.
        /// </summary>
        /// <param name="followTarget">Follow target to remove CombatText followers from</param>
        public static void DetachAllFromFollowTarget(Transform followTarget)
        {
            List<CombatTextInstance> children = new List<CombatTextInstance>(followTarget.GetComponentsInChildren<CombatTextInstance>(true));
            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetParent(children[i].Parent);
            }
        }
        /// <summary>
        /// Turns the display of all text on or off.
        /// </summary>
        /// <param name="show">Display text?</param>
        public void ShowText(bool show)
        {
            _showText = show;
            if (!_showText)
            {
                foreach (CombatTextPool pool in _poolDict.Values)
                {
                    for (int i = 0; i < pool.list.Count; i++)
                    {
                        pool.list[i].Despawn();
                    }
                }
            }
        }
        public void Reset()
        {
            styleAssetPath = "CombatTextStyles/";
            _textPrefab = Resources.Load<GameObject>("Prefabs/CombatTextInstance");
        }
    }
    public class CombatTextPool
    {
        public List<CombatTextInstance> list;
        public Transform parent;
        public int index = 0;
        public CombatTextPool(CombatTextStyle style)
        {
            list = new List<CombatTextInstance>();
            parent = new GameObject(style.name).transform;
        }
    }
}