using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACController : MonoBehaviour {

    public abstract void handleKeyDown(KeyCode key);

    public abstract void handleLClick();
}
