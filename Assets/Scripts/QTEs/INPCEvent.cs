using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCEvent
{
    IEnumerator Succeed();
    IEnumerator Failed();

    void HandleOnSuccess();
    void HandleOnFailure();
}
