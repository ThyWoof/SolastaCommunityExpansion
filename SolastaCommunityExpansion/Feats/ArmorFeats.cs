﻿using System;
using System.Collections.Generic;
using SolastaCommunityExpansion.Builders;
using SolastaCommunityExpansion.Builders.Features;
using SolastaModApi.Infrastructure;
using static FeatureDefinitionAttributeModifier;
using static RuleDefinitions;
using static SolastaModApi.DatabaseHelper.ArmorCategoryDefinitions;
using static SolastaModApi.DatabaseHelper.FeatureDefinitionDamageAffinitys;

namespace SolastaCommunityExpansion.Feats
{
    internal static class ArmorFeats
    {
        public static readonly Guid ArmorNamespace = new("d37cf3a0-6dbe-461f-8af5-58761414ef6b");

        public static void CreateArmorFeats(List<FeatDefinition> feats)
        {
            var lightArmorProficiency = BuildProficiency("FeatLightArmorProficiency",
                ProficiencyType.Armor, EquipmentDefinitions.LightArmorCategory);

            var mediumArmorProficiency = BuildProficiency("FeatMediumArmorProficiency",
                ProficiencyType.Armor, EquipmentDefinitions.MediumArmorCategory, EquipmentDefinitions.ShieldCategory);

            var dexterityModifier = BuildAttributeModifier("FeatDexIncrement",
                AttributeModifierOperation.Additive, AttributeDefinitions.Dexterity, 1);

            var strengthModifier = BuildAttributeModifier("FeatStrengthIncrement",
                AttributeModifierOperation.Additive, AttributeDefinitions.Strength, 1);

            var lightArmorFeat = BuildFeat("FeatLightArmor", lightArmorProficiency, dexterityModifier);

            // Note: medium armor feats have pre-req of light armor
            var mediumDexArmorFeat = BuildFeat("FeatMediumArmorDex", LightArmorCategory, mediumArmorProficiency, dexterityModifier);
            var mediumStrengthArmorFeat = BuildFeat("FeatMediumArmorStrength", LightArmorCategory, mediumArmorProficiency, strengthModifier);

            // Note: heavy armor master has pre-req of heavy armor
            var heavyArmorMasterFeat = BuildFeat("FeatHeavyArmorMasterClass", HeavyArmorCategory,
                DamageAffinityBludgeoningResistance, DamageAffinitySlashingResistance, DamageAffinityPiercingResistance);

            feats.AddRange(lightArmorFeat, mediumDexArmorFeat, mediumStrengthArmorFeat, heavyArmorMasterFeat);
        }

        public static FeatDefinition BuildFeat(string name, ArmorCategoryDefinition prerequisite, params FeatureDefinition[] features)
        {
            return FeatDefinitionBuilder
                .Create(name, ArmorNamespace)
                .SetGuiPresentation(Category.Feat)
                .SetFeatures(features)
                .SetArmorProficiencyPrerequisite(prerequisite)
                .AddToDB();
        }

        public static FeatDefinition BuildFeat(string name, params FeatureDefinition[] features)
        {
            return FeatDefinitionBuilder
                .Create(name, ArmorNamespace)
                .SetGuiPresentation(Category.Feat)
                .SetFeatures(features)
                .AddToDB();
        }

        public static FeatureDefinitionProficiency BuildProficiency(string name, ProficiencyType type, params string[] proficiencies)
        {
            return FeatureDefinitionProficiencyBuilder
                .Create(name, ArmorNamespace)
                .SetGuiPresentation(Category.Feat)
                .SetProficiencies(type, proficiencies)
                .AddToDB();
        }

        public static FeatureDefinitionAttributeModifier BuildAttributeModifier(string name, AttributeModifierOperation modifierType, string attribute, int amount)
        {
            return FeatureDefinitionAttributeModifierBuilder
                .Create(name, ArmorNamespace)
                .SetGuiPresentation(Category.Feat)
                .SetModifier(modifierType, attribute, amount)
                .AddToDB();
        }
    }
}
