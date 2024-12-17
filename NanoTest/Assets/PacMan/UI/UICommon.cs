using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class UICommon
    {
    }

    [Serializable]
    public class PauseMunuRef
    {
        public UnityEngine.GameObject root;
        public UnityEngine.UI.Text menuText;
        public UnityEngine.UI.Button continueBtn;
        public UnityEngine.UI.Button restartBtn;
        public UnityEngine.UI.Button quitBtn;
    }
}
