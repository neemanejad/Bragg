using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Character
{
    public partial class Character : Area2D
    {
        [Signal]
        public delegate void RemoveHealthEventHandler();
        [Signal]
        public delegate void AddHealthSignalEventHandler();
        [Signal]
        public delegate void LostAllHealthSignalEventHandler();
    }
}

