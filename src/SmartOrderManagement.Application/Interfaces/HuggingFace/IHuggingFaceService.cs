using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Interfaces.HuggingFace
{
    public interface IHuggingFaceService
    {
        Task<string> CallLLMWithRAG(string augmentedPrompt, string token);
    }
}
