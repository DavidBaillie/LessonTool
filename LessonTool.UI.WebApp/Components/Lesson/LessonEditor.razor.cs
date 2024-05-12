using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.FormModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using LessonTool.UI.WebApp.Extensions;
using LessonTool.UI.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;

namespace LessonTool.UI.WebApp.Components.Lesson
{
    public partial class LessonEditor
    {
        [Inject]
        private IAuthenticationStateHandler authenticationStateHandler { get; set; }

        [Parameter]
        public Guid LessonId { get; set; }

        [Parameter]
        public ILessonRepository LessonRepository { get; set; }

        [Parameter]
        public ISectionRepository SectionRepository { get; set; }

        private LessonFormModel lesson = new();
        private EditContext editContext = new(new object());

        private bool lessonFailedToLoad = false;
        private bool userIsAllowedToEdit = false;
        private string failureMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            userIsAllowedToEdit = (await authenticationStateHandler.GetSecurityTokenAsync(cancellationToken))
                .TokenHasAnyClaims([UserClaimConstants.Admin,  UserClaimConstants.Teacher]);

            try
            {
                if (LessonId == Guid.Empty)
                {
                    lesson = new()
                    {
                        VisibleDate = DateTime.UtcNow,
                        PlannedDate = DateTime.UtcNow,
                    };
                }
                else
                {
                    var dto = await LessonRepository.GetAsync(LessonId, cancellationToken);
                    lesson = dto.ToLessonFormModel(dto.Sections);
                }
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

            LessonDto dto = lesson.ToLessonDto(lesson.Sections);

            try
            {
                if (lesson.Id == Guid.Empty)
                {
                    var apiLesson = await LessonRepository.CreateAsync(dto, cancellationToken);
                    dto.Sections.ForEach(x =>  x.LessonId = apiLesson.Id);

                    var saveSectionTasks = dto.Sections.Select(x => SectionRepository.CreateAsync(x, cancellationToken));
                    await Task.WhenAll(saveSectionTasks);
                }
                else
                {
                    var apiLesson = await LessonRepository.UpdateAsync(dto, cancellationToken);
                    dto.Sections.ForEach(x => x.LessonId = apiLesson.Id);

                    var sectionSaves = dto.Sections.Select(x =>
                    {
                        if (x.Id == Guid.Empty)
                            return SectionRepository.CreateAsync(x, cancellationToken);

                        return SectionRepository.UpdateAsync(x, cancellationToken);
                    });
                    await Task.WhenAll(sectionSaves);
                }

                navigationManager.NavigateTo("/lessons");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Failed to save lesson");
            }
        }
    }
}