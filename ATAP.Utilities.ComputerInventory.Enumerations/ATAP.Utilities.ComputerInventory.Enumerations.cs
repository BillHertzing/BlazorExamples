
/* This assembly defines enumerations for the application
 * For  demo purposes, there are three. The first has elements with no attributes, the second has elements with a [Description] attribute. The third has elements with both [Description] and [SpecializedDescription] attributes.
 * 
 * [Description] is a "standard" attribute for enumeration values
 * [SpecializedDescription] is a custom attribute
 */
using ServiceStack.Text;
using ServiceStack.Text.EnumMemberSerializer;
using System;


namespace ATAP.Utilities.ComputerInventory.Enumerations
{

    
    public enum MainBoardMaker
    {
        Generic,
        ASUS,
        MSI
    }

    public enum CPUMaker
    {
        [Description("Generic-D")]
        Generic,
        [Description("Intel-D")]
        Intel,
        [Description("AMD-D")]
        AMD
    }

    public enum GPUMaker
    {
        [SpecializedDescription("Generic-SD")]
        [Description("Generic-D")]
        Generic,
        [SpecializedDescription("AMD-SD")]
        [Description("AMD-D")]
        AMD,
        [SpecializedDescription("NVIDEA-SD")]
        [Description("NVIDEA-D")]
        NVIDEA
    }
}
