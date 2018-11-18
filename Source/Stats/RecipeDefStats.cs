﻿using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;

namespace InGameDefEditor.Stats
{
	public class RecipeDefStats : DefStat<RecipeDef>, IParentStat
	{
		public float workAmount;
		public bool allowMixingIngredients;
		public bool autoStripCorpses;
		public bool productHasIngredientStuff;
		public int targetCountAdjustment;
		public float workSkillLearnFactor;
		public bool hideBodyPartNames;
		public bool isViolation;
		public float surgerySuccessChanceFactor;
		public float deathOnFailedSurgeryChance;
		public bool targetsBodyPart;
		public bool anesthetize;
		public bool dontShowIfAnyIngredientMissing;
		//[Unsaved]
		//private RecipeWorker workerInt;
		//[Unsaved]
		//private RecipeWorkerCounter workerCounterInt;
		//[Unsaved]
		//private IngredientValueGetter ingredientValueGetterInt;
		//public ConceptDef conceptLearned;

		public EffecterDefStat effectWorking;

		public ThingFilterStats fixedIngredientFilter;
		public ThingFilterStats defaultIngredientFilter;

		public DefStat<ResearchProjectDef> researchPrerequisite;
		public DefStat<WorkTypeDef> requiredGiverWorkType;
		public DefStat<ThingDef> unfinishedThingDef;
		public DefStat<SoundDef> soundWorking;
		public DefStat<StatDef> workSpeedStat;
		public DefStat<StatDef> efficiencyStat;
		public DefStat<StatDef> workTableEfficiencyStat;
		public DefStat<StatDef> workTableSpeedStat;
		public DefStat<HediffDef> addsHediff;
		public DefStat<HediffDef> removesHediff;
		public DefStat<SkillDef> workSkill;

		public List<SpecialProductType> specialProducts;

		public List<DefStat<SpecialThingFilterDef>> forceHiddenSpecialFilters;
		//public List<DefStat<ThingDef>> recipeUsers;
		public List<DefStat<BodyPartDef>> appliedOnFixedBodyParts;

		public List<IntValueDefStat<ThingDef>> products;
		public List<IntValueDefStat<SkillDef>> skillRequirements;
		public List<IngredientCountStats> ingredients;

		//[Unsaved]
		//private List<ThingDef> premultipliedSmallIngredients;
		//public List<string> factionPrerequisiteTags;
		//[MustTranslate]
		//public string successfullyRemovedHediffMessage;
		//private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		public RecipeDefStats(RecipeDef def) : base(def)
		{
			this.workAmount = def.workAmount;
			this.allowMixingIngredients = def.allowMixingIngredients;
			this.autoStripCorpses = def.autoStripCorpses;
			this.productHasIngredientStuff = def.productHasIngredientStuff;
			this.targetCountAdjustment = def.targetCountAdjustment;
			this.workSkillLearnFactor = def.workSkillLearnFactor;
			this.hideBodyPartNames = def.hideBodyPartNames;
			this.isViolation = def.isViolation;
			this.surgerySuccessChanceFactor = def.surgerySuccessChanceFactor;
			this.deathOnFailedSurgeryChance = def.deathOnFailedSurgeryChance;
			this.targetsBodyPart = def.targetsBodyPart;
			this.anesthetize = def.anesthetize;
			this.dontShowIfAnyIngredientMissing = def.dontShowIfAnyIngredientMissing;

			this.effectWorking = new EffecterDefStat(def.effectWorking);

			this.fixedIngredientFilter = new ThingFilterStats(def.fixedIngredientFilter);
			this.defaultIngredientFilter = new ThingFilterStats(def.defaultIngredientFilter);

			Util.TryAssignStatDef(def.researchPrerequisite, out this.researchPrerequisite);
			Util.TryAssignStatDef(def.requiredGiverWorkType, out this.requiredGiverWorkType);
			Util.TryAssignStatDef(def.unfinishedThingDef, out this.unfinishedThingDef);
			Util.TryAssignStatDef(def.soundWorking, out this.soundWorking);
			Util.TryAssignStatDef(def.workSpeedStat, out this.workSpeedStat);
			Util.TryAssignStatDef(def.efficiencyStat, out this.efficiencyStat);
			Util.TryAssignStatDef(def.workTableEfficiencyStat, out this.workTableEfficiencyStat);
			Util.TryAssignStatDef(def.workTableSpeedStat, out this.workTableSpeedStat);
			Util.TryAssignStatDef(def.addsHediff, out this.addsHediff);
			Util.TryAssignStatDef(def.removesHediff, out this.removesHediff);
			Util.TryAssignStatDef(def.workSkill, out this.workSkill);

			if (def.specialProducts == null)
				def.specialProducts = new List<SpecialProductType>(0);
			this.specialProducts = Util.CreateList(def.specialProducts);

			if (def.forceHiddenSpecialFilters == null)
				def.forceHiddenSpecialFilters = new List<SpecialThingFilterDef>(0);
			this.forceHiddenSpecialFilters = Util.CreateDefStatList(def.forceHiddenSpecialFilters);

			/*if (def.recipeUsers == null)
				def.recipeUsers = new List<ThingDef>(0);
			this.recipeUsers = Util.CreateDefStatList(def.recipeUsers);*/

			if (def.appliedOnFixedBodyParts == null)
				def.appliedOnFixedBodyParts = new List<BodyPartDef>();
			this.appliedOnFixedBodyParts = Util.CreateDefStatList(def.appliedOnFixedBodyParts);

			this.products = new List<IntValueDefStat<ThingDef>>((def.products != null) ? def.products.Count : 0);
			if (def.products != null)
				foreach (var v in def.products)
					this.products.Add(new IntValueDefStat<ThingDef>(v.thingDef) { value = v.count });

			this.skillRequirements = new List<IntValueDefStat<SkillDef>>((def.skillRequirements != null) ? def.skillRequirements.Count : 0);
			if (def.skillRequirements != null)
				foreach (var v in def.skillRequirements)
					this.skillRequirements.Add(new IntValueDefStat<SkillDef>(v.skill) { value = v.minLevel });

			this.ingredients = new List<IngredientCountStats>((def.ingredients != null) ? def.ingredients.Count : 0);
			if (def.ingredients != null)
				foreach (var v in def.ingredients)
					this.ingredients.Add(new IngredientCountStats(v));
		}

		internal void PreSave(RecipeDef d)
		{
			d.fixedIngredientFilter.ResolveReferences();
			d.defaultIngredientFilter.ResolveReferences();
			foreach (var v in d.ingredients)
				v.ResolveReferences();
		}

		public void ApplyStats(Def def)
		{
			if (def is RecipeDef d)
			{
				d.workAmount = this.workAmount;
				d.allowMixingIngredients = this.allowMixingIngredients;
				d.autoStripCorpses = this.autoStripCorpses;
				d.productHasIngredientStuff = this.productHasIngredientStuff;
				d.targetCountAdjustment = this.targetCountAdjustment;
				d.workSkillLearnFactor = this.workSkillLearnFactor;
				d.hideBodyPartNames = this.hideBodyPartNames;
				d.isViolation = this.isViolation;
				d.surgerySuccessChanceFactor = this.surgerySuccessChanceFactor;
				d.deathOnFailedSurgeryChance = this.deathOnFailedSurgeryChance;
				d.targetsBodyPart = this.targetsBodyPart;
				d.anesthetize = this.anesthetize;
				d.dontShowIfAnyIngredientMissing = this.dontShowIfAnyIngredientMissing;

				d.effectWorking = new EffecterDef();
				this.effectWorking.ApplyStats(d.effectWorking);

				d.fixedIngredientFilter = new ThingFilter();
				this.fixedIngredientFilter.ApplyStats(d.fixedIngredientFilter);

				d.defaultIngredientFilter = new ThingFilter();
				this.defaultIngredientFilter.ApplyStats(d.defaultIngredientFilter);

				Util.TryAssignDef(this.researchPrerequisite, out d.researchPrerequisite);
				Util.TryAssignDef(this.requiredGiverWorkType, out d.requiredGiverWorkType);
				Util.TryAssignDef(this.unfinishedThingDef, out d.unfinishedThingDef);
				Util.TryAssignDef(this.soundWorking, out d.soundWorking);
				Util.TryAssignDef(this.workSpeedStat, out d.workSpeedStat);
				Util.TryAssignDef(this.efficiencyStat, out d.efficiencyStat);
				Util.TryAssignDef(this.workTableEfficiencyStat, out d.workTableEfficiencyStat);
				Util.TryAssignDef(this.workTableSpeedStat, out d.workTableSpeedStat);
				Util.TryAssignDef(this.addsHediff, out d.addsHediff);
				Util.TryAssignDef(this.removesHediff, out d.removesHediff);
				Util.TryAssignDef(this.workSkill, out d.workSkill);

				d.specialProducts = Util.CreateList(this.specialProducts);

				/* TODO
				 * if (d.forceHiddenSpecialFilters == null)
					d.forceHiddenSpecialFilters = new List<SpecialThingFilterDef>();
				else
					d.forceHiddenSpecialFilters.Clear();
				foreach (var v in this.forceHiddenSpecialFilters)
				{
					SpecialThingFilterDef
				}*/

				//d.forceHiddenSpecialFilters = Util.CreateDefStatList(this.forceHiddenSpecialFilters);

				/*
				 * TODO Doubt this is needed
				if (this.recipeUsers == null)
				{
					if (d.recipeUsers != null)
						d.recipeUsers.Clear();
				}*/

				d.appliedOnFixedBodyParts = Util.ConvertDefStats(this.appliedOnFixedBodyParts);

				d.products.Clear();
				foreach (var v in this.products)
					d.products.Add(new ThingDefCountClass()
					{
						thingDef = v.Def,
						count = v.value
					});

				d.skillRequirements.Clear();
				foreach (var v in this.skillRequirements)
					d.skillRequirements.Add(new SkillRequirement()
					{
						skill = v.Def,
						minLevel = v.value
					});

				d.ingredients.Clear();
				foreach (var v in this.ingredients)
				{
					ThingFilter tf = new ThingFilter();
					v.ThingFilterStats.ApplyStats(tf);
					var i = new IngredientCount()
					{
						filter = tf
					};
					IngredientCountStats.SetIngredientCount(i, v.Count);
					d.ingredients.Add(i);
				}
			}
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;

			Util.InitializeDefStat(researchPrerequisite);
			Util.InitializeDefStat(requiredGiverWorkType);
			Util.InitializeDefStat(unfinishedThingDef);
			Util.InitializeDefStat(soundWorking);
			Util.InitializeDefStat(workSpeedStat);
			Util.InitializeDefStat(efficiencyStat);
			Util.InitializeDefStat(workTableEfficiencyStat);
			Util.InitializeDefStat(workTableSpeedStat);
			Util.InitializeDefStat(addsHediff);
			Util.InitializeDefStat(removesHediff);
			Util.InitializeDefStat(workSkill);

			Util.InitializeDefStat(forceHiddenSpecialFilters);
			//Util.InitializeDefStat(recipeUsers);
			Util.InitializeDefStat(appliedOnFixedBodyParts);

			Util.InitializeDefStat(products);
			Util.InitializeDefStat(skillRequirements);

			return true;
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) &&
				obj is RecipeDefStats s)
			{
				return
					this.workAmount == s.workAmount &&
					this.allowMixingIngredients == s.allowMixingIngredients &&
					this.autoStripCorpses == s.autoStripCorpses &&
					this.productHasIngredientStuff == s.productHasIngredientStuff &&
					this.targetCountAdjustment == s.targetCountAdjustment &&
					this.workSkillLearnFactor == s.workSkillLearnFactor &&
					this.hideBodyPartNames == s.hideBodyPartNames &&
					this.isViolation == s.isViolation &&
					this.surgerySuccessChanceFactor == s.surgerySuccessChanceFactor &&
					this.deathOnFailedSurgeryChance == s.deathOnFailedSurgeryChance &&
					this.targetsBodyPart == s.targetsBodyPart &&
					this.anesthetize == s.anesthetize &&
					this.dontShowIfAnyIngredientMissing == s.dontShowIfAnyIngredientMissing &&
					object.Equals(this.effectWorking, s.effectWorking) &&
					object.Equals(this.fixedIngredientFilter, s.fixedIngredientFilter) &&
					object.Equals(this.defaultIngredientFilter, s.defaultIngredientFilter) &&
					Util.AreEqual(this.researchPrerequisite, s.researchPrerequisite) &&
					Util.AreEqual(this.requiredGiverWorkType, s.requiredGiverWorkType) &&
					Util.AreEqual(this.unfinishedThingDef, s.unfinishedThingDef) &&
					Util.AreEqual(this.soundWorking, s.soundWorking) &&
					Util.AreEqual(this.workSpeedStat, s.workSpeedStat) &&
					Util.AreEqual(this.efficiencyStat, s.efficiencyStat) &&
					Util.AreEqual(this.workTableEfficiencyStat, s.workTableEfficiencyStat) &&
					Util.AreEqual(this.workTableSpeedStat, s.workTableSpeedStat) &&
					Util.AreEqual(this.addsHediff, s.addsHediff) &&
					Util.AreEqual(this.removesHediff, s.removesHediff) &&
					Util.AreEqual(this.workSkill, s.workSkill) &&
					Util.AreEqual(this.specialProducts, s.specialProducts);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}