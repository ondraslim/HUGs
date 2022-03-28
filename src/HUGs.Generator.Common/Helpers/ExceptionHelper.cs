using System;
using System.Text;

namespace HUGs.Generator.Common.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetCompleteExceptionMessage(Exception ex)
        {
            var sb = new StringBuilder();
            while (ex is not null)
            {
                sb.Append(ex.Message);
                sb.AppendLine(";");
                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}