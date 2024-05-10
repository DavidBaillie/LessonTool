using Microsoft.AspNetCore.Components;
using LessonTool.UI.WebApp.Extensions;
using LessonTool.UI.Application.Interfaces;
using LessonTool.Common.Domain.Interfaces;

namespace LessonTool.UI.WebApp.Pages
{
    public partial class LessonEditPage
    {
        [Inject]
        private IFullLessonRepository fullLessonRepository { get; set; }

        [Inject]
        private ISectionRepository sectionRepository { get; set; }

        private Guid lessonId = Guid.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (navigationManager.TryGetQueryString("Id", out Guid id, new(Guid.TryParse)))
                lessonId = id;
        }
    }
}