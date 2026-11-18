using System;
using UnityEngine;

namespace Puto.UseFlow.Home
{
    public class ButtonHomeBehaviour : MonoBehaviour
    {

        public bool twoStep = true;
        

        private void OnEnable()
        {
            if (!twoStep)
            {
                GetComponent<Animator>().Play("active");
            }
        }

        public void OnClicked()
        {
            GetComponent<Animator>().Play("show");
        }
    }
}
