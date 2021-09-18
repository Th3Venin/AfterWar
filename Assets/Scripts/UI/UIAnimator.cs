using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public Animator blackBarsAnimator;
    public Animator titleCardAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            blackBarsAnimator.SetTrigger("HideBars");
            titleCardAnimator.SetTrigger("HideTitleCard");
        }
    }
}
