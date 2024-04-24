using MediatR;
using TH.Services;
using TH.Services.Models;

namespace teachers_hours_be.Application.Commands;

public static class GetReportsFile
{
    public record Query(GetTeachersHoursReportRequest Request) : IRequest<FileResult>;

    internal class Handler : IRequestHandler<Query, FileResult>
    {
        public Task<FileResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var generator = new GenerateTeachersHoursReport();
            return generator.Run(request.Request);
            // TODO: возвращает файл сгенерированного отчета
            // TODO: добавить в Service логику генерации отчета
            // TODO: добавить контроллер
            // добавить nswag для fe и общение с Апи
        }
    }
}
