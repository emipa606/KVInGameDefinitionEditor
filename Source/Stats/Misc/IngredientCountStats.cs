﻿using System;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	public class IngredientCountStats
	{
		public readonly ThingFilterStats ThingFilterStats;
		public float Count = 0;

		public IngredientCountStats(IngredientCount i)
		{
			this.ThingFilterStats = new ThingFilterStats(i.filter);
			this.Count = GetIngredientCount(i);
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is IngredientCountStats s)
			{
				return
					this.Count == s.Count &&
					object.Equals(this.ThingFilterStats, s.ThingFilterStats);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override string ToString()
		{
			return
				this.GetType().Name + Environment.NewLine +
				"    ThingFilterStats: " + this.ThingFilterStats + Environment.NewLine +
				"    Count: " + this.Count;
		}

		public static float GetIngredientCount(IngredientCount i)
		{
			return (float)typeof(IngredientCount).GetField("count", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(i);
		}

		public static void SetIngredientCount(IngredientCount i, float f)
		{
			typeof(IngredientCount).GetField("count", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(i, f);
		}
	}
}
