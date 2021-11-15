using System.Collections.Generic;

namespace ApplicationCore.Dtos
{
    public record SimpleResultDto(bool IsSuccess, IDictionary<string, string> ErrorMessages = null);
}
