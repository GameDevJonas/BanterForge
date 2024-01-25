using UnityEngine;

namespace GDPanda.BanterForge
{
    public class ExampleDialogueActionListener : DialogueActionListener
    {
        public void DebugLogInteger(int i)
        {
            Debug.Log(i);
        }
    }
}