using LessonTool.Common.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using LessonTool.UI.WebApp.Extensions;

namespace LessonTool.UI.WebApp.Pages
{
    public partial class LessonEditPage
    {
        [Inject]
        public ILessonRepository LessonRepository { get; set; }

        [Inject]
        public ISectionRepository SectionRepository { get; set; }

        private Guid lessonId = Guid.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (navigationManager.TryGetQueryString("Id", out Guid id, new(Guid.TryParse)))
                lessonId = id;
        }
    }
}