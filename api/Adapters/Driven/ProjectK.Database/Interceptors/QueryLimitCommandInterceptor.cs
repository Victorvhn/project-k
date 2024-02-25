using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProjectK.Database.Interceptors;

public class QueryLimitCommandInterceptor(IConfiguration configuration) : IDbCommandInterceptor
{
    private readonly int _queryLimit = configuration.GetValue<int?>("ProjectK.Api:PaginationCountLimit") ?? 100;

    public InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        const string limitPattern = @"\bLIMIT\s+@__p_\d+";
        const string parameterPattern = "@__p_\\d+";

        if (Regex.IsMatch(command.CommandText, limitPattern))
        {
            var limitMatch = Regex.Match(command.CommandText, limitPattern);
            var parameterMatch = Regex.Match(limitMatch.Value, parameterPattern);

            if (parameterMatch.Success)
            {
                var parameterName = parameterMatch.Value;
                foreach (DbParameter parameter in command.Parameters)
                    if (parameter.ParameterName == parameterName)
                    {
                        var value = Convert.ToInt32(parameter.Value);

                        parameter.Value = value > _queryLimit ? _queryLimit : value;
                        break;
                    }
            }
        }

        return result;
    }
}