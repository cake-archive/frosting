private static string GetEnvironmentValueOrArgument(ICakeContext context, string environmentVariable, string argumentName)
{
    var arg = context.EnvironmentVariable(environmentVariable);
    if(string.IsNullOrWhiteSpace(arg))
    {
        arg = context.Argument<string>(argumentName, null);
    }
    return arg;
}