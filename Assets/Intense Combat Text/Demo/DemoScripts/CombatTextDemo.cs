using UnityEngine;
using System.Collections.Generic;

namespace DevPlz.CombatText.Demo
{
    //This script is only used to control the Combat Text demo scene.
    //It and the entire Demo Scene folder can be safely deleted.
    public class CombatTextDemo : MonoBehaviour
    {
        public Vector3 pos = new Vector3(0, 0, 0);
        public TextStyle styleKey = TextStyle.DamagePlayer;
        public Transform target;
        public Transform followTarget;
        public Transform dialogueTarget;
        public List<CombatTextStyle> styleList = new List<CombatTextStyle>();
        public CombatTextStyle dialogueStyle;
        private CombatTextStyle selectedStyle;
        private List<string> dialogue = new List<string>() { "Hello!", "This can also be used\n for NPC dialogue", "HELP!",
        "Hail, Adventurer!\nI am in need of your skills.\nPlease kill gnolls and gather\n 10 pairs of their rollerskates,\n also known as 'gnollerskates,'\n and return them to me." };
        private int stringindex;
        private int styleindex;
        public bool rapid;
        private void Start()
        {
            selectedStyle = styleList[0];
            InvokeRepeating("FollowTargetText", .25f, .5f);
            InvokeRepeating("TargetText", .5f, .5f);
            InvokeRepeating("Dialogue", 1f, 5f);
        }
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity)) return;

            if (Input.GetMouseButtonDown(0) && !rapid) Spawn(hit.point);
            if (Input.GetMouseButton(0) && rapid) Spawn(hit.point);

        }
        private void Spawn(Vector3 point)
        {
            string text = "";
            switch (styleindex)
            {
                case 5:
                    text = "DODGE";
                    break;
                case 6:
                    text = "MISS";
                    break;
                default:
                    text = Random.Range(1, 9999).ToString();
                    break;
            }
            CombatText.Spawn(selectedStyle, text, point);

        }
        private void TargetText()
        {
            CombatText.Spawn(styleKey, Random.Range(1, 9999).ToString(), target.transform.position);
        }
        private void FollowTargetText()
        {
            if (!followTarget) return;
            CombatText.Spawn(styleKey, Random.Range(1, 9999).ToString(), followTarget.transform.position, followTarget);
        }
        private void Dialogue()
        {
            CombatText.Spawn(dialogueStyle, dialogue[stringindex], dialogueTarget.transform.position, dialogueTarget);
            stringindex++;
            if (stringindex >= dialogue.Count) stringindex = 0;
        }

        public void SetStyle(int style)
        {
            styleindex = style;
            selectedStyle = styleList[style];
        }
    }
}
