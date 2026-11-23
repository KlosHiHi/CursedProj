using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;

namespace OxideClassLibrary.Extensions
{
    public static class DraftExtension
    {
        extension(Draft draft)
        {
            public DraftDto? ToDto()
                => draft is null ? null : new()
                {
                    DraftId = draft.DraftId,
                    Title = draft.Title,
                    CategoryId = draft.CategoryId,
                    CreationTime = draft.CreationTime,
                    LastChangeTime = draft.LastChangeTime,
                };
        }

        extension(IEnumerable<Draft> drafts)
        {
            public IEnumerable<DraftDto?> ToDtos()
                => drafts.Select(ToDto);
        }
    }
}
