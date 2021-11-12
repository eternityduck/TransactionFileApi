using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.TransactionsCqrs.Commands.ImportFile
{
    public class ImportFileCommand : IRequest
    {
        public IFormFile File { get; set; }

        public ImportFileCommand(IFormFile file) =>
            File = file;
    }
}