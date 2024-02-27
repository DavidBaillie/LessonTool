using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages
{
    public partial class LessonViewPage
    {
        [Inject]
        private IFullLessonRepository lessonRepository {  get; set; }   
        
        private LessonDto lesson = new();

        private string failureMessage = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                if (navigationManager.TryGetQueryString("id", out Guid id, Guid.TryParse))
                {
                    lesson = await lessonRepository.LoadFullLessonFromSourceAsync(id, cancellationToken);
                }
            }
            catch (HttpRequestException ex) 
            {
                failureMessage = $"Failed to load lesson: {ex.StatusCode}";
            }
        }
    }
}