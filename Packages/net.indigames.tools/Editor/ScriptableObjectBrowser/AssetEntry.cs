﻿using System;
using Object = UnityEngine.Object;

namespace ScriptableObjectBrowser
{
    [Serializable]
    public class AssetEntry
    {
        public string Path = String.Empty;
        public string RPath = String.Empty;
        public string Name = String.Empty;
        public Object Asset;
        public int MatchAmount;
        public bool Visible = true;
        public bool Selected = false;
    }
}