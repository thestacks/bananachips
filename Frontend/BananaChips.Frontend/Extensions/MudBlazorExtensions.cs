using MudBlazor;

namespace BananaChips.Frontend.Extensions;

public static class MudBlazorExtensions
{
    public static (int Skip, int Take) CalculatePagination(this TableState tableState)
    {
        var skip = (tableState.Page - 1) * tableState.PageSize;
        if (skip < 0)
            skip = 0;

        return (skip, tableState.PageSize);
    }
}