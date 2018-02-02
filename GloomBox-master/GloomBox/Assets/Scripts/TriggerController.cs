#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region TriggerController Class
public class TriggerController : MonoBehaviour
{
	// This is a superclass for all triggers
	// Functions within this class will be overridden

    public virtual void OnTrigger()
    {
    }

    public virtual void TriggerOff()
    {
    }
}
#endregion