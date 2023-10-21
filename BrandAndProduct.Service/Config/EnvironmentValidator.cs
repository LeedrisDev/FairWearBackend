namespace BrandAndProduct.Service.Config;

/// <summary>
/// Utility class to validate required environment variables.
/// </summary>
public class EnvironmentValidator
{
    /// <summary>
    /// Validates the required environment variables.
    /// </summary>
    /// <exception cref="Exception">Thrown if the required environment variables are not defined.</exception>
    public static void ValidateRequiredVariables()
    {
        DisplayBeginValidationMessage();

        var varEnvDefined = IsVarEnvDefined("GOODONYOU_SCRAPPER_SERVICE_HOST");
        varEnvDefined &= IsVarEnvDefined("GOODONYOU_SCRAPPER_SERVICE_PORT_HTTP");
        varEnvDefined &= IsVarEnvDefined("BRAND_AND_PRODUCT_DB_SERVICE_HOST");
        varEnvDefined &= IsVarEnvDefined("BRAND_AND_PRODUCT_DB_SERVICE_PORT_HTTP");
        varEnvDefined &= IsVarEnvDefined("PRODUCT_DATA_RETRIEVER_SERVICE_HOST");
        varEnvDefined &= IsVarEnvDefined("PRODUCT_DATA_RETRIEVER_SERVICE_PORT_HTTP");


        if (!varEnvDefined)
            throw new Exception("The required environment variables are not defined.");
    }

    /// <summary>
    /// Checks if the specified environment variable is defined.
    /// </summary>
    /// <param name="variableName">The name of the environment variable to check.</param>
    /// <returns>True if the variable is defined; otherwise, false.</returns>
    private static bool IsVarEnvDefined(string variableName)
    {
        var variable = Environment.GetEnvironmentVariable(variableName);
        DisplayIsVariableDefined(variableName, !string.IsNullOrEmpty(variable));
        return !string.IsNullOrEmpty(variable);
    }

    /// <summary>
    /// Displays the begin validation message.
    /// </summary>
    private static void DisplayBeginValidationMessage()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("###############################################");
        Console.WriteLine("## Validating required environment variables ##");
        Console.WriteLine("###############################################\n");
        Console.ResetColor();
    }

    /// <summary>
    /// Displays whether the variable is defined or not.
    /// </summary>
    /// <param name="variableName">The name of the environment variable.</param>
    /// <param name="isDefined">True if the variable is defined; otherwise, false.</param>
    private static void DisplayIsVariableDefined(string variableName, bool isDefined)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"Environment variable '{variableName}' : ");
        if (isDefined)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("OK");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NOT DEFINED");
        }

        Console.ResetColor();
    }
}