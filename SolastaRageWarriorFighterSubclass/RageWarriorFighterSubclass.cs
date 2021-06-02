using SolastaModApi;
using SolastaModApi.Extensions;
using static FeatureDefinitionSavingThrowAffinity;

namespace SolastaRageWarriorFighterSubclass
{
    public static class SolastaRageWarriorFighterSubclass
    {
        const string RageWarriorFighterSubclassName = "RageWarriorFighterSubclass";
        const string RageWarriorFighterSubclassNameGuid = "51079583-cdeb-42c7-b914-800092c3fe6d";

        public static void BuildAndAddSubclass()
        {
            var subclassGuiPresentation = new GuiPresentationBuilder(
                    "Subclass/&RageWarriorFighterSubclassDescription",
                    "Subclass/&RageWarriorFighterSubclassTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.DomainElementalFire.GuiPresentation.SpriteReference)
                    .Build();

            var definition = new CharacterSubclassDefinitionBuilder(RageWarriorFighterSubclassName, RageWarriorFighterSubclassNameGuid)
                    .SetGuiPresentation(subclassGuiPresentation)
                    .AddFeatureAtLevel(DatabaseHelper.FeatureDefinitionPowers.PowerReckless, 3)
                    .AddFeatureAtLevel(RageClassPowerBuilder.RageClassPower, 3)
                    .AddFeatureAtLevel(FastMovementMovementAffinityBuilder.FastMovementMovementAffinity, 7)
                    .AddFeatureAtLevel(FastMovementMovementPowerForLevelUpDescriptionBuilder.FastMovementMovementPowerForLevelUpDescription, 7)
                    .AddFeatureAtLevel(DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierMartialChampionImprovedCritical, 10)
                    .AddToDB();

            //FeatureDefinitionMovementAffinity
            DatabaseHelper.FeatureDefinitionMovementAffinitys.MovementAffinityLongstrider.ToString();

            DatabaseHelper.FeatureDefinitionSubclassChoices.SubclassChoiceFighterMartialArchetypes.Subclasses.Add(definition.Name);
        }
    }

    internal class FastMovementMovementAffinityBuilder : BaseDefinitionBuilder<FeatureDefinitionMovementAffinity>
    {
        const string FastMovementMovementAffinityName = "AHFastMovementMovementAffinity";
        const string FastMovementMovementAffinityNameGuid = "a47c486a-b2fa-4151-aeaa-c891429cbecb";

        protected FastMovementMovementAffinityBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionMovementAffinitys.MovementAffinityLongstrider, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&FastMovementMovementAffinityTitle";
            Definition.GuiPresentation.Description = "Feature/&FastMovementMovementAffinityDescription";
        }

        public static FeatureDefinitionMovementAffinity CreateAndAddToDB(string name, string guid)
            => new FastMovementMovementAffinityBuilder(name, guid).AddToDB();

        public static FeatureDefinitionMovementAffinity FastMovementMovementAffinity
            => CreateAndAddToDB(FastMovementMovementAffinityName, FastMovementMovementAffinityNameGuid);
    }

    internal class FastMovementMovementPowerForLevelUpDescriptionBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string FastMovementMovementAffinityName = "AHFastMovementMovementPowerForLevelUpDescription";
        const string FastMovementMovementAffinityNameGuid = "b11e5253-9984-4f13-ba07-032e2dcf6a40";

        protected FastMovementMovementPowerForLevelUpDescriptionBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPowers.PowerDomainElementalFireBurst, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&FastMovementMovementAffinityTitle";
            Definition.GuiPresentation.Description = "Feature/&FastMovementMovementAffinityDescription";

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.None); //Short rest for the subclass
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.Permanent);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(0);
            Definition.SetShortTitleOverride("Feature/&FastMovementMovementAffinityTitle");
            Definition.SetEffectDescription(new EffectDescription());
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new FastMovementMovementPowerForLevelUpDescriptionBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower FastMovementMovementPowerForLevelUpDescription
            => CreateAndAddToDB(FastMovementMovementAffinityName, FastMovementMovementAffinityNameGuid);
    }

    internal class RageClassPowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string RageClassPowerName = "AHRageClassPower";
        const string RageClassPowerNameGuid = "2d8c3567-7cce-4723-8980-9fa64d3fc609";

        protected RageClassPowerBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPowers.PowerDomainElementalFireBurst, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&RageClassPowerTitle";
            Definition.GuiPresentation.Description = "Feature/&RageClassPowerDescription";

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.ShortRest); //Short rest for the subclass
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(1);
            Definition.SetShortTitleOverride("Feature/&RageClassPowerTitle");

            //Create the power attack effect
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = RageClassConditionBuilder.RageClassCondition;

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(rageEffect);
            newEffectDescription.HasSavingThrow = false;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Self);
            newEffectDescription.SetCanBePlacedOnCharacter(true);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new RageClassPowerBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            => CreateAndAddToDB(RageClassPowerName, RageClassPowerNameGuid);
    }

    internal class RageClassConditionBuilder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string RageClassConditionName = "AHRageClassCondition";
        const string RageClassConditionNameGuid = "fc3ee331-5a2f-4c7c-9d9c-5b73374d2590";

        protected RageClassConditionBuilder(string name, string guid) : base(DatabaseHelper.ConditionDefinitions.ConditionHeraldOfBattle, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&RageClassConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&RageClassConditionDescription";

            Definition.SetAllowMultipleInstances(false);
            Definition.Features.Clear();
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityConditionBullsStrength);
            Definition.Features.Add(RageClassStrengthSavingThrowAffinityBuilder.RageClassStrengthSavingThrowAffinity);
            Definition.Features.Add(RageClassDamageBonusAttackModifierBuilder.RageClassDamageBonusAttackModifier);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);


            Definition.SetDurationType(RuleDefinitions.DurationType.Turn);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new RageClassConditionBuilder(name, guid).AddToDB();

        public static ConditionDefinition RageClassCondition
            => CreateAndAddToDB(RageClassConditionName, RageClassConditionNameGuid);
    }

    internal class RageClassStrengthSavingThrowAffinityBuilder : BaseDefinitionBuilder<FeatureDefinitionSavingThrowAffinity>
    {
        const string RageClassStrengthSavingThrowAffinityName = "AHRageClassStrengthSavingThrowAffinity";
        const string RageClassStrengthSavingThrowAffinityNameGuid = "7324770d-4c80-4357-a8fa-5ca2b39ecd72";

        protected RageClassStrengthSavingThrowAffinityBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionSavingThrowAffinitys.SavingThrowAffinityCreedOfArun, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&RageClassStrengthSavingThrowAffinityTitle";
            Definition.GuiPresentation.Description = "Feature/&RageClassStrengthSavingThrowAffinityDescription";

            Definition.AffinityGroups.Clear();
            var strengthSaveAffinityGroup = new SavingThrowAffinityGroup();
            strengthSaveAffinityGroup.affinity = RuleDefinitions.CharacterSavingThrowAffinity.Advantage;
            strengthSaveAffinityGroup.abilityScoreName = "Strength";
        }

        public static FeatureDefinitionSavingThrowAffinity CreateAndAddToDB(string name, string guid)
            => new RageClassStrengthSavingThrowAffinityBuilder(name, guid).AddToDB();

        public static FeatureDefinitionSavingThrowAffinity RageClassStrengthSavingThrowAffinity
            => CreateAndAddToDB(RageClassStrengthSavingThrowAffinityName, RageClassStrengthSavingThrowAffinityNameGuid);
    }

    internal class RageClassDamageBonusAttackModifierBuilder : BaseDefinitionBuilder<FeatureDefinitionAttackModifier>
    {
        const string RageClassDamageBonusAttackModifierName = "AHRageClassDamageBonusAttackModifier";
        const string RageClassDamageBonusAttackModifierNameGuid = "46b23166-30d8-47bc-8e63-1d492f938471";

        protected RageClassDamageBonusAttackModifierBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAttackModifiers.AttackModifierFightingStyleArchery, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&RageClassDamageBonusAttackModifierTitle";
            Definition.GuiPresentation.Description = "Feature/&RageClassDamageBonusAttackModifierDescription";

            Definition.SetAttackRollModifier(0);
            Definition.SetDamageRollModifier(2);//Could find a way to up this at level 9 to match barb but that seems like a lot of work right now :)
        }

        public static FeatureDefinitionAttackModifier CreateAndAddToDB(string name, string guid)
            => new RageClassDamageBonusAttackModifierBuilder(name, guid).AddToDB();

        public static FeatureDefinitionAttackModifier RageClassDamageBonusAttackModifier
            => CreateAndAddToDB(RageClassDamageBonusAttackModifierName, RageClassDamageBonusAttackModifierNameGuid);
    }
}
