using System.Diagnostics.CodeAnalysis;

namespace ProjectK.Api.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method)]
internal class TransactionAttribute : Attribute;