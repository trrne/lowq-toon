﻿using System;

namespace Toon.Extend
{
    [Serializable]
    public class NanmoHaittehenwaException : Exception
    {
        public NanmoHaittehenwaException() : base("1つはいれろやエクセプション") { }
        public NanmoHaittehenwaException(string msg) : base(msg) { }
    }
}