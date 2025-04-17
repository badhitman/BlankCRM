////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SharedLib
{
    /// <summary>
    /// Глобальные утилиты
    /// </summary>
    public static partial class GlobalToolsStandart
    {
        /// <summary>
        /// Добавить информация об исключении
        /// </summary>
        public static void InjectException(this List<ResultMessage> sender, List<ValidationResult> validationResults)
            => sender.AddRange(validationResults.Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage)).Select(x => new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = x.ErrorMessage! }));

        /// <summary>
        /// Добавить информация об исключении
        /// </summary>
        public static void InjectException(this List<ResultMessage> sender, Exception ex)
        {
            sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = ex.Message });
            if (ex.StackTrace != null)
                sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = ex.StackTrace });
            int i = 0;
            while (ex.InnerException != null)
            {
                i++;
                sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = $"InnerException -> {i}/ {ex.InnerException.Message}" });
                if (ex.InnerException.StackTrace != null)
                    sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = $"InnerException -> {i}/ {ex.InnerException.StackTrace}" });

                ex = ex.InnerException;
            }
        }
    }
}
