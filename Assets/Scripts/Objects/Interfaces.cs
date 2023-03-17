using System.Collections.Generic;

public interface ILiving
{
    /// <summary>
    /// Returns the living entity's attribute holder.
    /// </summary>
    /// <returns>An attribute holder.</returns>
    AttributeHolder GetAttributes();
}