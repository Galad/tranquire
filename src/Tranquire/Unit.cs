﻿using System;

namespace Tranquire
{
    /// <summary>
    /// Represent a type with a single value. It is used to represent void methods.
    /// </summary>
    //[System.Diagnostics.CodeAnalysis.]
    public struct Unit : IEquatable<Unit>
    {
#pragma warning disable CS1591,CA1801
        public static Unit Default { get; } = default;
        public bool Equals(Unit other) => true;
        public override bool Equals(object obj) => obj is Unit;
        public static bool operator ==(Unit left, Unit right) => true;
        public static bool operator !=(Unit left, Unit right) => false;
        public override int GetHashCode() => 0;
        public override string ToString() => "unit";
#pragma warning restore CS1591,CA1801
    }
}
