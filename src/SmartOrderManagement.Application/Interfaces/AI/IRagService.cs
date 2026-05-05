using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Interfaces.AI
{
    public interface IRagService
    {
        Task<string> GetAugmentedPromptAsync(string originalPrompt);
    }
}
