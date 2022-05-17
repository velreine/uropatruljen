using System.Text;
using System.Globalization;

namespace HubApi.Logic;

/**
 * Provides convenience methods for manipulating strings.
 */
public static class StringConverter
{

    /**
     * Super naive implementation that just replaces cases of uppercase letters with 
     */
    public static string ToSnakeCase(string input)
    {
        // Much faster than string concatenation in most use cases.
        var sb = new StringBuilder();

        // Since snake_case does not mean the input should begin with an underscore return if the length is less than 2.
        if (input.Length < 2)
        {
            return input;
        }
        
        // The first letter should just be the lower-case variant of the char.
        sb.Append(char.ToLower(input[0]));
        
        // From the second letter and onwards, replace upper-case letters with the
        // lower case variant prepended with underscore.
        for (var i = 1; i < input.Length; i++)
        {

            var current = input[i];

            // Checks if the next char (if available) is upper-case, defaults to false.
            // This is useful for abbreviations,
            // E.g. SMTPServer will become smtp_server and not s_m_t_p_server
            var nextIsUpperCase = (i < input.Length - 1) && input[i + 1] == char.ToUpper(input[i + 1]);

            if (char.ToUpper(current) == input[i] && !nextIsUpperCase)
            {
                sb.Append($"_{char.ToLower(current)}");
            }
            else
            {
                sb.Append(char.ToLower(current));
            }


        }

        return sb.ToString();
    }
    
    
    
}