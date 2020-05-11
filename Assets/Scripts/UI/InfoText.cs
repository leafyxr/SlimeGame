using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewInfoText",menuName ="InfoText")]
public class InfoText : ScriptableObject
{
    public string Header;
    [TextArea(3, 10)]
    public string _InfoText;
    public string ConfirmText;
}
