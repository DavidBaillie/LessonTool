using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.FormModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Threading;
using LessonTool.UI.WebApp.Extensions;

namespace LessonTool.UI.WebApp.Components.Lesson
{
    public partial class LessonEditor
    {
        [Parameter]
        public Guid LessonId { get; set; }

        [Parameter]
        public ILessonRepository LessonRepository { get; set; }

        [Parameter]
        public ISectionRepository SectionRepository { get; set; }

        private LessonFormModel lesson = new();
        private EditContext editContext = new(new object());

        private bool lessonFailedToLoad = false;
        private string failureMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                var lessonLoadTask = LessonRepository.GetAsync(LessonId, cancellationToken);
                var sectionsLoadTask = SectionRepository.GetSectionsByLessonAsync(LessonId, cancellationToken);

                await Task.WhenAll(sectionsLoadTask, lessonLoadTask);

                LessonDto dto = lessonLoadTask.Result;
                dto.Sections = sectionsLoadTask.Result;

                lesson = dto.ToLessonFormModel();
                editContext = new(lesson);
            }
            catch (HttpRequestException ex)
            {
                lessonFailedToLoad = true;
                failureMessage = $"Failed to load the desired lesson: {ex.StatusCode}";
            }
        }

        private void AddSection()
        {
            if (lessonFailedToLoad)
                return;

            lesson.Sections.Add(new());
            StateHasChanged();
        }

        private async Task TrySaveFormModelAsync()
        {
            if (lessonFailedToLoad)
                return;

            if (!editContext.Validate())
                return;

            LessonDto dto = lesson.ToLessonDto();

            if (lesson.Id == Guid.Empty)
                await LessonRepository.CreateAsync(dto, cancellationToken);
            else
                await LessonRepository.UpdateAsync(dto, cancellationToken);
        }
    }
}