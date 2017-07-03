namespace Helper.Extensions
{
    /// <summary>
    /// Defines a textual boolean value
    /// </summary>
    /// <remarks />
    public enum BooleanText
    {
        [BooleanText(TrueValue = "Accepted", FalseValue = "Declined")]
        AcceptedDeclined,

        [BooleanText(TrueValue = "Active", FalseValue = "Inactive")]
        ActiveInactive,

        [BooleanText(TrueValue = "Checked", FalseValue = "Unchecked")]
        CheckedUnchecked,

        [BooleanText(TrueValue = "Correct", FalseValue = "Incorrect")]
        CorrectIncorrent,

        [BooleanText(TrueValue = "Enabled", FalseValue = "Disabled")]
        EnabledDisabled,

        [BooleanText(TrueValue = "On", FalseValue = "Off")]
        OnOff,

        [BooleanText(TrueValue = "Yes", FalseValue = "No")]
        YesNo
    }
}
