/***************************************************************************\
Project:      Daily Rewards
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using System;

public enum dailyRewardUnit { MONEY, COINS }

namespace NiobiumStudios
{
    /**
    * The class representation of the Reward
    **/
    [Serializable]
    public class Reward
    {
        public dailyRewardUnit unit;
        public float reward;
        public Sprite sprite;
    }
}