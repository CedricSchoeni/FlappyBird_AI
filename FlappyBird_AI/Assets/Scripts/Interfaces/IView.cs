using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView {

    void setText(string name, string text, bool concat = false);
    void activate(string name, bool active);

}
