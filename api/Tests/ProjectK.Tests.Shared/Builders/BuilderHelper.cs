using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace ProjectK.Tests.Shared.Builders;

public static class BuilderHelper
{
    public static TEntity InstantiateClass<TEntity>() where TEntity : class
    {
        // Let's assume that the class has a parameterless constructor because of EF Core
        var instance = (TEntity)Activator.CreateInstance(typeof(TEntity), true)!;

        return instance;
    }

    public static TEntity SetPropertyValue<TEntity, TValue>(
        this TEntity target,
        Expression<Func<TEntity, TValue>> memberLambda, TValue value)
        where TEntity : class
    {
        var body = memberLambda.Body;
        var memberLambdaIsMemberExpression = IsMemberExpression(body);

        if (!memberLambdaIsMemberExpression)
            return target;

        var memberSelectorExpression = GetMemberExpression(body);
        var propertyInfo = memberSelectorExpression?.Member as PropertyInfo;

        if (propertyInfo == null)
            return target;

        FieldInfo? field;
        // This is a workaround for forcing the Id property to be set
        if (propertyInfo.Name == "Id")
        {
            var method = propertyInfo.GetGetMethod(true);
            var instructions = method
                .GetInstructions()
                .FirstOrDefault(w => w.OpCode == OpCodes.Ldfld);

            if (instructions is null) throw new Exception("Backing field not found");

            field = (FieldInfo)instructions.Operand;
        }
        else
        {
            field = target.GetType()
                .GetField($"<{propertyInfo.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        field?.SetValue(target, value);

        return target;
    }

    private static bool IsMemberExpression(Expression body)
    {
        return body is MemberExpression || IsUnaryExpression(body);
    }

    private static bool IsUnaryExpression(Expression body)
    {
        return body is UnaryExpression;
    }

    private static MemberExpression? GetMemberExpression(Expression body)
    {
        return IsUnaryExpression(body)
            ? GetMemberExpressionFromUnaryMember(body)
            : body as MemberExpression;
    }

    private static MemberExpression? GetMemberExpressionFromUnaryMember(Expression body)
    {
        return (body as UnaryExpression)?.Operand as MemberExpression;
    }
}