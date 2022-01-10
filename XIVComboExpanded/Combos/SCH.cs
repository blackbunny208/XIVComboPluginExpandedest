using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class SCH
    {
        public const byte ClassID = 15;
        public const byte JobID = 28;

        public const uint
            FeyBless = 16543,
            Consolation = 16546,
            EnergyDrain = 167,
            Aetherflow = 166,
            Indomitability = 3583,
            Lustrate = 189,
            Excogitation = 7434,
            SacredSoil = 188,
            SummonEos = 17215,
            SummonSelene = 17216,
            WhisperingDawn = 16537,
            FeyIllumination = 16538,
            Dissipation = 3587,
            Aetherpact = 7437,
            SummonSeraph = 16545,
            ChainStratagem = 7436,
            Recitation = 16542,
            EmergencyTactics = 3586,
            DeploymentTactics = 3585,
            Ruin2 = 17870;

        public static class Buffs
        {
            public const ushort
                Dissipation = 791,
                Recitation = 1896;
        }

        public static class Debuffs
        {
            public const ushort Placeholder = 0;
        }

        public static class Levels
        {
            public const byte
                ChainStratagem = 66,
                Recitation = 74;
        }
    }

    internal class ScholarFairyFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarFairyFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<SCHGauge>();
            if (!Service.BuddyList.PetBuddyPresent && gauge.SeraphTimer == 0 && !HasEffect(SCH.Buffs.Dissipation))
                return IsEnabled(CustomComboPreset.ScholarSeleneOption) ? SCH.SummonSelene : SCH.SummonEos;

            return actionID;
        }
    }

    internal class ScholarRuinChainFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarRuinChainFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            return IsActionOffCooldown(SCH.ChainStratagem) && GetCooldown(SCH.Ruin2).CooldownRemaining >= 0.5 && level >= SCH.Levels.ChainStratagem ? SCH.ChainStratagem : actionID;
        }
    }

    internal class ScholarSeraphConsolationFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarSeraphConsolationFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SCH.FeyBless)
            {
                var gauge = GetJobGauge<SCHGauge>();
                if (gauge.SeraphTimer > 0)
                    return SCH.Consolation;
            }

            return actionID;
        }
    }

    internal class ScholarExcogRecitationFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarExcogRecitationFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            return IsActionOffCooldown(SCH.Recitation) && level >= SCH.Levels.Recitation ? SCH.Recitation : actionID;
        }
    }

    internal class ScholarEnergyDrainFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarEnergyDrainFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SCH.EnergyDrain)
            {
                var gauge = GetJobGauge<SCHGauge>();
                if (gauge.Aetherflow == 0)
                    return SCH.Aetherflow;
            }

            if (IsEnabled(CustomComboPreset.ScholarEverythingFeature))
            {
                if (HasEffect(SCH.Buffs.Recitation) && (actionID == SCH.Indomitability || actionID == SCH.Excogitation))
                    return actionID;
                var gauge = GetJobGauge<SCHGauge>();
                if (gauge.Aetherflow == 0)
                    return SCH.Aetherflow;
            }

            return actionID;
        }
    }

    internal class ScholarLucidReminderFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarLucidReminderFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            return IsActionOffCooldown(All.LucidDreaming) && HasCondition(ConditionFlag.InCombat) && !IsActionOffCooldown(actionID) && LocalPlayer?.CurrentMp <= 9000 ? All.LucidDreaming : actionID;
        }
    }
}
