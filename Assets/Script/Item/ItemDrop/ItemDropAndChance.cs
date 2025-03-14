﻿using System;
using UnityEngine;

namespace Script.Item.ItemDrop
{
    [Serializable]
    public class ItemDropAndChance
    {
        public ItemData itemData;
        public int minDropNumber;
        public int maxDropNumber;
        [Range(0, 100)] public float chance;
    }
}