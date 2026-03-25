using System.Collections.Generic;

public class DamageCalculator {
    public static float Calculate(AttackContext context, List<IDamageModifier> modifiers = null) {
        float value = context.baseDamage;

        if (modifiers != null) {
            foreach (IDamageModifier modifier in modifiers) {
                value = modifier.Modify(context, value);
            }
        }

        return value;
    }
}